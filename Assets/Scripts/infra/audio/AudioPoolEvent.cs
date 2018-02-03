using UnityEngine;
using Infra.Utils;

namespace Infra.Audio {
/// <summary>
/// Plays a random audio event with random volume, pitch and stereo pan.
/// </summary>
[CreateAssetMenu(menuName = "Audio Events/Audio Pool")]
public class AudioPoolEvent : AudioEvent {
    public AudioClip[] clips;

    [MinMaxRange(0f, 2f)]
    public RangedFloat volume = new RangedFloat(1f);
    [MinMaxRange(0f, 2f)]
    public RangedFloat pitch = new RangedFloat(1f);
    [MinMaxRange(-1f, 1f)]
    public RangedFloat stereoPan;

    public override void Play(AudioSource source) {
        if (clips.Length == 0) return;

        source.clip = clips[Random.Range(0, clips.Length)];
        source.volume = volume.Random();
        source.pitch = pitch.Random();
        source.panStereo = stereoPan.Random();
        source.Play();
    }
}
}