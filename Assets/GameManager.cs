using System.Collections;
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

		[Header ("UI")]
		public UIManager UIMan;
		public UnityEngine.Video.VideoPlayer videoPlayer;

		[Header ("Music")]
		public HammersManager hammersManager;

		[Header ("Dusts")]
		public List<DustCharecter> dusts;

		[Header ("Twicking")]
		public int maxRounds;

		[Header ("Timing")]
		private float sharedTimer;
		public float videoLoadDelay;
		public float warningTime;
		public float timeAfterWin;

		[Header ("Debug")]
		public bool skipIntro;

		private bool gameRunning;
		private int curRound;
		private bool showWarning;

		private List<DustCharecter> livingDusts;

		// Use this for initialization
		void Start ()
		{
			Debug.Log ("START");
			livingDusts = new List<DustCharecter> ();
			TransitionToIntro ();
		}
	
		// Update is called once per frame
		void Update ()
		{
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
			}
		}

		void PreRoundUpdate ()
		{
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
					}
				}
			} else {
				if (sharedTimer != 0 && Time.time > sharedTimer) {
					curRound++;
					if (isGameOver ()) {
						TransitionToWinning ();
					} else {
						TransitionToPreRound ();
					}
				}
			}
		}

		void WinningUpdate ()
		{
			if (Input.GetKey (KeyCode.Space)) {
				UIMan.setState (States.WINNING, false);
				TransitionToIntro ();
			}
		}

		void TransitionToIntro(){
			Debug.Log ("Intro");
			curState = States.INTRO;
			gameRunning = false;
			livingDusts.Clear ();
			livingDusts.AddRange (dusts);
			curRound = 0;
			showWarning = true;
			UIMan.setState (States.INTRO, true);
			if (videoPlayer != null && !skipIntro) {
				videoPlayer.enabled = true;
				sharedTimer = Time.time + videoLoadDelay;
				videoPlayer.Play ();
			}
		}

		void TransitionToPreRound(){
			Debug.Log ("pre");
			curState = States.PRE_ROUND;
			sharedTimer = 0f;
			gameRunning = false;
			hammersManager.Stop ();
			UIMan.setState (States.PRE_ROUND, true);
			UIMan.setRound (curRound+1);
			foreach (DustCharecter dust in dusts) {
				dust.resetValues ();
			}
		}

		void TransitionToRound(){
			Debug.Log ("round");

			curState = States.ROUND;
			livingDusts.Clear ();
			livingDusts.AddRange (dusts);
			gameRunning = true;
			foreach (DustCharecter dust in dusts) {
				dust.startRound ();
			}
			if (!showWarning) {
				hammersManager.Play ();
			} else {
				sharedTimer = Time.time + warningTime;
				UIMan.setState (States.ROUND, true);
			}
		}

		void TransitionToWinning(){
			Debug.Log ("won");

			UIMan.setState (States.WINNING, true);
			curState = States.WINNING;
		}

		bool isGameOver(){
			if (curRound >= maxRounds)
				return true;
			List<int> Wins = new List<int>();
			foreach (DustCharecter dc in dusts) {
				Wins.Add (dc.getWins());
			}
			Wins.Sort ();
			if (maxRounds - curRound + Wins[1] < Wins[0]){
				return true;
			}
			return false;
		}
	}
}