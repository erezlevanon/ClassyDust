using UnityEngine;

namespace Infra.Audio {
/// <summary>
/// A base class that allows to play an audio. Each derived class can specify
/// another method of playing a sound.
/// </summary>
public abstract class AudioEvent : ScriptableObject {
    /// <summary>
    /// Play the audio using the AudioManager.
    /// </summary>
    public void Play() {
        var source = AudioManager.GetAvailableSoundChannel();
        if (source != null) {
            Play(source);
        }
    }

    /// <summary>
    /// Play the audio through the given AudioSource.
    /// </summary>
    public abstract void Play(AudioSource source);
}
}