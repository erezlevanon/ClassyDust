using UnityEngine;
using System.Collections.Generic;

namespace Infra.Audio {
public class AudioManager : MonoSingleton<AudioManager> {
    [SerializeField] float masterSoundVolume = 1f;

    [SerializeField] int soundSourcesToAddWhenNeeded = 5;
    [SerializeField] int maxSoundSources = 20;

    [SerializeField] GameObject _soundsSources;
    private int soundSourceIndex = 0;
    private List<AudioSource> soundsSources;


    protected override void Init() {
        soundsSources = new List<AudioSource>(_soundsSources.GetComponentsInChildren<AudioSource>());
        soundSourceIndex = 0;

        foreach (var soundsSource in soundsSources) {
            soundsSource.loop = false;
        }

        SetMasterSoundVolume(masterSoundVolume);
    }

    public static void PlayClip(AudioClip clip, float volume = 1f, float stereoPan = 0) {
        var index = Instance.GetAvailableSoundChannelIndex();
        var soundSource = Instance.soundsSources[index];
        soundSource.spatialBlend = 0f;
        _PlayClip(clip, volume, soundSource, stereoPan);
    }

    public static AudioSource GetAvailableSoundChannel() {
        var index = Instance.GetAvailableSoundChannelIndex();
        var soundSource = Instance.soundsSources[index];
        soundSource.spatialBlend = 0f;
        return soundSource;
    }

    public static void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume = 1f) {
        if (clip == null) return;
        int index = Instance.GetAvailableSoundChannelIndex();
        var soundSource = Instance.soundsSources[index];
        soundSource.transform.position = position;
        soundSource.spatialBlend = 1f;
        _PlayClip(clip, volume, soundSource);
    }

    private static void _PlayClip(AudioClip clip, float volume, AudioSource soundSource, float stereoPan = 0) {
        soundSource.clip = clip;
        soundSource.volume = volume;
        soundSource.panStereo = stereoPan;
        soundSource.Play();
    }

    public int GetAvailableSoundChannelIndex() {
        // Look for the next available sound source.
        for (int i = soundSourceIndex; i < soundsSources.Count; i++) {
            var soundSource = soundsSources[i];
            if (!soundSource.isPlaying) {
                soundSourceIndex = i;
                return soundSourceIndex;
            }
        }
        for (int i = 0; i < soundSourceIndex; i++) {
            var soundSource = soundsSources[i];
            if (!soundSource.isPlaying) {
                soundSourceIndex = i;
                return soundSourceIndex;
            }
        }

        var sourcesToAdd = Mathf.Min(soundSourcesToAddWhenNeeded, maxSoundSources - soundsSources.Count);
        if (sourcesToAdd <= 0) {
            // No available sources. Look for the most completed next source and
            // replace it.
            float maxProgress = 0f;
            for (int j = soundSourceIndex + 1; j <= 5; j++) {
                var i = j % soundsSources.Count;
                var soundSource = soundsSources[i];
                var duration = soundSource.clip == null ? 0f : soundSource.clip.length;
                if (duration <= 0) {
                    soundSourceIndex = i;
                    return soundSourceIndex;
                }
                var progress = soundSource.time / duration;
                if (progress > maxProgress) {
                    maxProgress = progress;
                    soundSourceIndex = i;
                }
            }
            return soundSourceIndex;
        }

        // All sound sources are being used. Create some new sources.
        soundSourceIndex = soundsSources.Count;
        var protoypeSource = soundsSources[0].gameObject;
        var parentTransform = protoypeSource.transform.parent;
        for (int i = 0; i < sourcesToAdd; i++) {
            var obj = Instantiate(protoypeSource);

            obj.transform.SetParent(parentTransform, false);
            obj.name = "Source" + (soundSourceIndex + i + 1);
            var newSource = obj.GetComponent<AudioSource>();
            newSource.playOnAwake = false;
            newSource.loop = false;
            soundsSources.Add(newSource);
        }

        return soundSourceIndex;
    }

    public static void SetMasterSoundVolume(float volume) {
        AudioListener.volume = volume;
    }

    public static void SetSoundVolume(float volume) {
        foreach (var soundSource in Instance.soundsSources) {
            soundSource.volume = volume;
        }
    }
}
}
