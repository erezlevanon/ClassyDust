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

		[Header ("Clips and Volumes")]
		public AudioClip activated;
		public float actVol;
		public AudioClip jump;
		public float jumpVol;
		public AudioClip land;
		public float landVol;
		public AudioClip turbo;
		public float turboVol;
		public AudioClip gethit;
		public float hitVol;
		public AudioClip crushed;
		public float crushedVol;

		private AudioSource audioSource;
		private Dictionary<Samples, AudioClip> audioDict;
		private Dictionary<Samples, float> volDict;


		public void Start() {
			audioSource = GetComponent<AudioSource> ();
			audioDict = new Dictionary<Samples, AudioClip> ();
			volDict = new Dictionary<Samples, float> ();
			audioDict [Samples.ACTIVATED] = activated;
			audioDict [Samples.JUMP] = jump;
			audioDict [Samples.LAND] = land;
			audioDict [Samples.TURBO] = turbo;
			audioDict [Samples.HIT] = gethit;
			audioDict [Samples.CRUSHED] = crushed;

			volDict [Samples.ACTIVATED] = actVol;
			volDict [Samples.JUMP] = jumpVol;
			volDict [Samples.LAND] = landVol;
			volDict [Samples.TURBO] = turboVol;
			volDict [Samples.HIT] = hitVol;
			volDict [Samples.CRUSHED] = crushedVol;
		}

		public void play(Samples sample) {
			if (!audioSource)
				return;
			if (audioSource.isPlaying)
				audioSource.Stop ();
			audioSource.clip = audioDict [sample];
			audioSource.volume = volDict [sample];
			audioSource.Play ();
		}

	}
}