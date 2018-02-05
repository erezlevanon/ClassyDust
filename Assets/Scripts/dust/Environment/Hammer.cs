using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dust
{
	public class Hammer : MonoBehaviour
	{

		public enum NOTE {
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



		private Animator animator;

		// Use this for initialization
		void Start ()
		{
			animator = GetComponent<Animator> ();
		}

		public NOTE getNote() {
			return note;
		}

		public void HitNote() {
			if (animator != null) 
				animator.SetTrigger ("Play");
		}

		public void playNote() {
			noteToPlay.Play ();
		}
	}
}