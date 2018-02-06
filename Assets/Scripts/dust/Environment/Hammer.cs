using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dust
{
	public class Hammer : MonoBehaviour
	{

		public enum NOTE
		{
			_3Cs,
			_3D,
			_3Ds,
			_3E,
			_3F,
			_3Fs,
			_3G,
			_3Gs,
			_4A,
			_4As,
			_4B,
			_4C,
			_4Cs,
			_4D,
			_4Ds,
			_4E,
			_4F,
			_4Fs,
			_4G,
			_4Gs,
			_5A,
			_5As,
			_5B,
			_5C,
			_5Cs,
			_5D,
			_5Ds,
		}

		public NOTE note;
		public AudioSource noteToPlay;
		public AnimationClip clip;

		private Queue<float> delayedNotes;

		private Animator animator;
		private float anim_length;

		// Use this for initialization
		void Start ()
		{
			delayedNotes = new Queue<float> ();
			animator = GetComponent<Animator> ();
			anim_length = clip.length;
		}

		public void Update ()
		{
			if (delayedNotes == null || delayedNotes.Count == 0) 
				return;
			if (animator != null && animator.GetCurrentAnimatorStateInfo (animator.GetLayerIndex ("Base Layer")).IsName ("idle")) {
				float normalizedTime = (Time.time - delayedNotes.Dequeue ()) / anim_length;
				if (normalizedTime > 1f)
					return;
				animator.Play (clip.name,
							   animator.GetLayerIndex ("Base Layer"),
							   normalizedTime);
			}
				
		}

		public NOTE getNote ()
		{
			return note;
		}

		public void HitNote ()
		{
			if (animator != null && animator.GetCurrentAnimatorStateInfo (animator.GetLayerIndex ("Base Layer")).IsName ("idle")) {
				animator.SetTrigger ("Play");
			} else {
				delayedNotes.Enqueue (Time.time);
			}
		}

		public void playNote ()
		{
			noteToPlay.Play ();
		}
	}
}