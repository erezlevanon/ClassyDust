﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmfLite;

namespace Dust
{
	public class HammersManager : MonoBehaviour
	{
		[Header ("Song")]
		public float speedRatio;
		public TextAsset sourceMidiFile;
		public int track;
		public float bpm;
	
		[Header ("Debug")]
		public bool play_song;

		private bool playing;

		private Dictionary<Hammer.NOTE, Hammer> hammersDict_;
		static private Dictionary<int, Hammer.NOTE> numToNote_ = new Dictionary<int, Hammer.NOTE>() {
			{ 40, Hammer.NOTE.CAUGHE},
			{ 41, Hammer.NOTE.BOO},
			{ 42, Hammer.NOTE.CLAPS},
			{ 43, Hammer.NOTE.APLAUSE},
			{ 49, Hammer.NOTE._3Cs },
			{ 50, Hammer.NOTE._3D },
			{ 51, Hammer.NOTE._3Ds },
			{ 52, Hammer.NOTE._3E },
			{ 53, Hammer.NOTE._3F },
			{ 54, Hammer.NOTE._3Fs },
			{ 55, Hammer.NOTE._3G },
			{ 56, Hammer.NOTE._3Gs },
			{ 57, Hammer.NOTE._4A },
			{ 58, Hammer.NOTE._4As },
			{ 59, Hammer.NOTE._4B },
			{ 60, Hammer.NOTE._4C },
			{ 61, Hammer.NOTE._4Cs },
			{ 62, Hammer.NOTE._4D },
			{ 63, Hammer.NOTE._4Ds },
			{ 64, Hammer.NOTE._4E },
			{ 65, Hammer.NOTE._4F },
			{ 66, Hammer.NOTE._4Fs },
			{ 67, Hammer.NOTE._4G },
			{ 68, Hammer.NOTE._4Gs },
			{ 69, Hammer.NOTE._5A },
			{ 70, Hammer.NOTE._5As },
			{ 71, Hammer.NOTE._5B },
			{ 72, Hammer.NOTE._5C },
			{ 73, Hammer.NOTE._5Cs },
			{ 74, Hammer.NOTE._5D },
			{ 75, Hammer.NOTE._5Ds },
		};

		MidiFileContainer song;
		MidiTrackSequencer sequencer;

		public void Play (){
			playing = true;
			sequencer = new MidiTrackSequencer (song.tracks [track], song.division, bpm);
			ApplyMessages (sequencer.Start ());
		}

		public void Stop() {
			playing = false;
		}


		// Use this for initialization
		IEnumerator Start ()
		{
			playing = false;
			hammersDict_ = new Dictionary<Hammer.NOTE, Hammer> ();
			foreach (Hammer hammer in FindObjectsOfType<Hammer>()) {
				hammersDict_ [hammer.getNote ()] = hammer;
			}
			song = MidiFileLoader.Load (sourceMidiFile.bytes);
			yield return new WaitForSeconds (1.0f);
		}

		void Update ()
		{
			if (sequencer != null && sequencer.Playing && playing) {
				ApplyMessages (sequencer.Advance (Time.deltaTime * speedRatio));
			}
		}

		void ApplyMessages (List<MidiEvent> messages)
		{
			if (messages != null && play_song) {
				foreach (var m in messages) {
					if ((m.status & 0xf0) == 0x90) {
						if (numToNote_.ContainsKey (m.data1)) {
							if (hammersDict_.ContainsKey (numToNote_ [m.data1])) {
								hammersDict_ [numToNote_ [m.data1]].HitNote ();
							}
						} else {
							Debug.Log (m.data1);
						}
					}
				}
			}
		}
	}
}