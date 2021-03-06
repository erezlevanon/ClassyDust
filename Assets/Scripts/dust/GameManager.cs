﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmfLite;


namespace Dust
{
	public class GameManager : MonoBehaviour
	{

		public enum States
		{
			INTRO,
			PRE_ROUND,
			ROUND,
			WINNING,
		};

		private States curState;
		private AudioSource audioPlayer;

		[Header ("UI")]
		public UIManager UIMan;
		public UnityEngine.Video.VideoPlayer videoPlayer;
		public BoxCollider2D podium;

		[Header ("Music And Sound")]
		public HammersManager hammersManager;
		public AudioClip playOnIntroOver;
		public AudioClip RoundStartFirst;
		public AudioClip RoundStartRest;
		public AudioClip RoundEnd;
		public AudioClip EndSong;


		[Header ("Dusts")]
		public List<DustCharecter> dusts;

		[Header ("Twicking")]
		public int maxRounds;

		[Header ("Timing")]
		private float sharedTimer;
		private float sharedTimer_1;
		public float videoLoadDelay;
		public float timeAfterAllMoving;
		public float warningTime;
		public float timeAfterWin;
		public float endSongDelay;
		public float IntroClipDelay;

		[Header ("Debug")]
		public States entryState;
		public bool skipIntro;

		// flags
		private bool gameRunning;
		private int curRound;
		private bool showWarning;
		private bool moveToRound;
		private bool videoStarted;
		private bool endsongflag;

		private List<DustCharecter> livingDusts;

		// Use this for initialization
		void Start ()
		{
			livingDusts = new List<DustCharecter> ();
			audioPlayer = GetComponent<AudioSource> ();
			gameRunning = false;
			livingDusts.Clear ();
			livingDusts.AddRange (dusts);
			curRound = 0;
			switch (entryState) {
			case States.INTRO:
				TransitionToIntro ();
				break;
			case States.PRE_ROUND:
				UIMan.setState (States.INTRO, false);
				UIMan.setState (States.PRE_ROUND, false);
				UIMan.setState (States.ROUND, false);
				UIMan.setState (States.WINNING, false);
				TransitionToPreRound ();
				break;
			case States.ROUND:
				UIMan.setState (States.INTRO, false);
				UIMan.setState (States.PRE_ROUND, false);
				UIMan.setState (States.ROUND, false);
				UIMan.setState (States.WINNING, false);

				curRound = 1;
				TransitionToRound ();
				break;
			case States.WINNING:
				UIMan.setState (States.INTRO, false);
				UIMan.setState (States.PRE_ROUND, false);
				UIMan.setState (States.ROUND, false);
				TransitionToWinning ();
				break;
			default:
				TransitionToIntro ();
				break;
			}
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (Input.GetKey (KeyCode.Escape)) {
				UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene ();
				UnityEngine.SceneManagement.SceneManager.LoadScene (scene.name);
			}
			switch (curState) {
			case States.INTRO:
				IntroUpdate ();
				break;
			case States.PRE_ROUND:
				PreRoundUpdate ();
				break;
			case States.ROUND:
				RoundUpdate ();
				break;
			case States.WINNING:
				WinningUpdate ();
				break;
			default:
				return;
			}
		}

		void IntroUpdate ()
		{
			if (skipIntro || (!videoPlayer.isPlaying && Time.time > sharedTimer)) {
				UIMan.setState (States.INTRO, false);
				videoPlayer.enabled = false;
				TransitionToPreRound ();
				return;
			}
			if (!videoStarted) {
				if (videoPlayer.frame > 1) {
					videoStarted = true;
					sharedTimer_1 = Time.time + IntroClipDelay;
				}
			} else {
				if (Time.time > sharedTimer_1) {
					playSample (playOnIntroOver, 0.2f);
					sharedTimer_1 += 1000f;
				}
			}
		}

		void PreRoundUpdate ()
		{
			int moving = 0;
			foreach (DustCharecter d in dusts) {
				if (d.isMoving ())
					moving++;
			}
			if (moving == dusts.Count) {
				if (!moveToRound) {
					moveToRound = true;
					sharedTimer = Time.time + timeAfterAllMoving;
				} else if (Time.time > sharedTimer) {
					UIMan.setState (States.PRE_ROUND, false);
					TransitionToRound ();
					return;
				}
			}
			if (Input.GetKey (KeyCode.Space)) {
				UIMan.setState (States.PRE_ROUND, false);
				TransitionToRound ();
			}
		}

		void RoundUpdate ()
		{
			if (gameRunning) {
				if (showWarning) {
					if (Time.time >= sharedTimer) {
						hammersManager.Play ();
						UIMan.setState (States.ROUND, false);
						showWarning = false;
					}
					return;
				} else {
					foreach (DustCharecter dust in dusts) {
						if (!dust.IsAlive () && livingDusts.Contains (dust)) {
							livingDusts.Remove (dust);
						}
					}
					if (livingDusts.Count <= 1) {
						gameRunning = false;
						hammersManager.Stop ();
						sharedTimer = Time.time + timeAfterWin;
						foreach (DustCharecter d in livingDusts) {
							d.winRound ();
						}
					}
				}
			} else {
				if (endsongflag && Time.time > sharedTimer -endSongDelay) {
					playSample (EndSong, 0.5f);
					endsongflag = false;
				}
				if (sharedTimer != 0 && Time.time > sharedTimer) {
					curRound++;
					if (isGameOver ()) {
						TransitionToWinning ();
					} else {
						playSample (RoundEnd, 0.2f);
						TransitionToPreRound ();
					}
				}
			}
		}

		void WinningUpdate ()
		{
			if (Input.GetKey (KeyCode.Space)) {
				UIMan.setState (States.WINNING, false);
				audioPlayer.Stop ();
				TransitionToIntro ();
			}
		}

		void TransitionToIntro(){
			audioPlayer.Stop ();
			curState = States.INTRO;
			gameRunning = false;
			livingDusts.Clear ();
			livingDusts.AddRange (dusts);
			videoStarted = false;
			curRound = 0;
			showWarning = true;
			moveToRound = false;
			podium.enabled = false;
			endsongflag = true;
			UIMan.setState (States.INTRO, true);
			foreach (DustCharecter dust in dusts) {
				dust.resetWins ();
				dust.showWins (false);
				dust.freeze ();
			}
			if (videoPlayer != null && !skipIntro) {
				videoPlayer.enabled = true;
				sharedTimer = Time.time + videoLoadDelay;
				videoPlayer.Play ();
			}
		}

		void TransitionToPreRound(){
			curState = States.PRE_ROUND;
			sharedTimer = 0f;
			gameRunning = false;
			podium.enabled = false;
			hammersManager.Stop ();
			UIMan.setState (States.PRE_ROUND, true);
			UIMan.setRound (curRound+1);
			foreach (DustCharecter dust in dusts) {
				dust.resetValues ();
				dust.unfreeze ();
				dust.showWins (false);
			}
		}

		void TransitionToRound(){

			curState = States.ROUND;
			livingDusts.Clear ();
			livingDusts.AddRange (dusts);
			gameRunning = true;
			podium.enabled = false;
			foreach (DustCharecter dust in dusts) {
				dust.startRound ();
			}
			if (!showWarning) {
				playSample (RoundStartRest, 1f);
				hammersManager.Play ();
			} else {
				sharedTimer = Time.time + warningTime;
				playSample (RoundStartFirst, 1f);
				UIMan.setState (States.ROUND, true);
			}
		}

		void TransitionToWinning(){
			UIMan.setRound (0);
			int maxwins = 0;
			foreach (DustCharecter d in dusts) {
				if (d.getWins () > maxwins)
					maxwins = d.getWins ();
			}
			foreach (DustCharecter dust in dusts) {
				dust.resetValues ();
				dust.showArrows (false);
				dust.readyForEnding ();
				if (dust.getWins() == maxwins) {
					dust.winGame ();
				} else {
					dust.loseGame ();
				}
				dust.showWins (true);
				dust.freeze ();
			}
			podium.enabled = true;
			UIMan.setState (States.WINNING, true);
			curState = States.WINNING;
		}

		bool isGameOver(){
			if (curRound >= maxRounds)
				return true;
			return false;
		}

		private void playSample (AudioClip sample, float vol) {
			audioPlayer.Stop ();
			audioPlayer.volume = vol;
			audioPlayer.clip = sample;
			audioPlayer.Play ();
		}
	}
}