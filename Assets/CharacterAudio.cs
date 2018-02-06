using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dust
{

	public class CharacterAudio : MonoBehaviour
	{
		

		public enum Samples {
			ACTIVATED,
			JUMP,
			LAND,
			TURBO,
			HIT,
			CRUSHED,
		}

		public AudioClip activated;
		public AudioClip jump;
		public AudioClip land;
		public AudioClip turbo;
		public AudioClip gethit;
		public AudioClip crushed;

		private AudioSource audioSource;
		private Dictionary<Samples, AudioClip> audioDict;

		public void Start() {
			audioSource = GetComponent<AudioSource> ();
			audioDict = new Dictionary<Samples, AudioClip> ();
			audioDict [Samples.ACTIVATED] = activated;
			audioDict [Samples.JUMP] = jump;
			audioDict [Samples.LAND] = land;
			audioDict [Samples.TURBO] = turbo;
			audioDict [Samples.HIT] = gethit;
			audioDict [Samples.CRUSHED] = crushed;
		}

		public void play(Samples sample) {
			if (!audioSource)
				return;
			if (audioSource.isPlaying)
				audioSource.Stop ();
			audioSource.clip = audioDict [sample];
			audioSource.Play ();
		}

	}
}