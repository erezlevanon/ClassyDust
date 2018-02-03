using UnityEngine;
using System;
using Infra.Utils;

namespace Infra.Gameplay {
public class EndlessWorldScroller : MonoBehaviour {
    public event Action<Transform> OnChunkMoved;

    [Tooltip("Scrolling will be performed in attempt to keep the target in the center of the world")]
    [SerializeField] Transform target;
    [Tooltip("It is recommended to use 3 chunks")]
    [SerializeField] Transform[] chunks;

    [Tooltip("The index of the center chunk")]
    [SerializeField] int index;

    [Tooltip("Set to 0 to calculate the size using the distance between the chunks")]
    [SerializeField] float chunkSize;
    [Tooltip("The offset from the target to move the back chunk to the front or vice versa. Set to 0 to calculate the trigger using the chunk size")]
    [SerializeField] float offsetTrigger;

    public float ChunkSize {
        get {
            return chunkSize;
        }
    }

    private int indexFromCenterToFront;
    private int indexFromCenterToBack;
    private float backToFrontDistance;

    protected void Awake() {
        indexFromCenterToFront = chunks.Length - 1 - index;
        indexFromCenterToBack = index;
        if (Mathf.Approximately(chunkSize, 0)) {
            chunkSize = Mathf.Abs(chunks[1].position.x - chunks[0].position.x);
        }
        if (Mathf.Approximately(offsetTrigger, 0)) {
            offsetTrigger = chunkSize / 2;
        }
        backToFrontDistance = chunkSize * chunks.Length;
    }

    public void UpdateNow() {
        while (MoveIfNeeded()) {}
    }

    public void ResetChunks() {
        if (OnChunkMoved != null) {
            for (int i = 0; i < chunks.Length; i++) {
                var chunk = chunks[MathsUtils.Mod(index - indexFromCenterToBack + i, chunks.Length)];
                OnChunkMoved(chunk);
            }
        }
    }

    protected void Update() {
        MoveIfNeeded();
    }

    private bool MoveIfNeeded() {
        var chunk = chunks[index];
        var offset = target.position.x - chunk.position.x;
        if (offset > offsetTrigger) {
            // Target is ahead move back chunk forward.
            var backChunk = chunks[MathsUtils.Mod(index - indexFromCenterToBack, chunks.Length)];
            backChunk.Translate(backToFrontDistance, 0, 0);
            if (OnChunkMoved != null) {
                OnChunkMoved(backChunk);
            }
            index = (index + 1) % chunks.Length;
            return true;
        } else if (offset < -offsetTrigger) {
            // Target is behind move front chunk backward.
            var frontChunk = chunks[(index + indexFromCenterToFront) % chunks.Length];
            frontChunk.Translate(-backToFrontDistance, 0, 0);
            if (OnChunkMoved != null) {
                OnChunkMoved(frontChunk);
            }
            index = MathsUtils.Mod(index - 1, chunks.Length);
            return true;
        }
        return false;
    }
}
}
