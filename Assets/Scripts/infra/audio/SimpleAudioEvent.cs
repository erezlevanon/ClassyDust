using UnityEngine;

namespace Infra.Audio {
[CreateAssetMenu(menuName = "Audio Events/Simple")]
public class SimpleAudioEvent : AudioEvent {
    public AudioClip clip;
    [Range(0f, 10f)]
    public float volume = 1f;
    [Range(0f, 2f)]
    public float pitch = 1f;
    [Range(-1f, 1f)]
    public float stereoPan;

    public override void Play(AudioSource source) {
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.panStereo = stereoPan;
        source.Play();
    }
}
}