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
		public float timeAfterWin;
		public int maxRounds;

		[Header ("Debug")]
		public bool skipIntro;

		private float timeToReset;
		private float videoLoadDelay;

		private bool gameRunning;
		private int curRound;

		private List<DustCharecter> livingDusts;

		// Use this for initialization
		void Start ()
		{
			Debug.Log ("START");
			curRound = 0;
			gameRunning = false;
			livingDusts = new List<DustCharecter> ();
			livingDusts.AddRange (dusts);
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
			if (skipIntro || (!videoPlayer.isPlaying && Time.time > videoLoadDelay)) {
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
				foreach (DustCharecter dust in dusts) {
					if (!dust.IsAlive () && livingDusts.Contains (dust)) {
						livingDusts.Remove (dust);
					}
				}
				if (livingDusts.Count <= 1) {
					gameRunning = false;
					hammersManager.Stop ();
					timeToReset = Time.time + timeAfterWin;
				}
			} else {
				if (timeToReset != 0 && Time.time > timeToReset) {
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
			curRound = 0;
			UIMan.setState (States.INTRO, true);
			if (videoPlayer != null && !skipIntro) {
				videoPlayer.enabled = true;
				videoLoadDelay = Time.time + 2f;
				videoPlayer.Play ();
			}
		}

		void TransitionToPreRound(){
			Debug.Log ("pre");
			curState = States.PRE_ROUND;
			timeToReset = 0f;
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
			UIMan.setState (States.ROUND, true);
			hammersManager.Play ();
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