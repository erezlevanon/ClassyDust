using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmfLite;


namespace Dust
{
	public class GameManager : MonoBehaviour
	{

		private enum States
		{
			INTRO,
			PRE_ROUND,
			ROUND,
			WINNING,
		};

		private States curState;

		public HammersManager hammersManager;
		public UIManager UIMan;

		public List<DustCharecter> dusts;

		public float timeAfterWin;
		private float timeToReset;

		private bool gameRunning;

		private List<DustCharecter> livingDusts;

		// Use this for initialization
		void Start ()
		{
			gameRunning = false;
			livingDusts = new List<DustCharecter> ();
			livingDusts.AddRange (dusts);
			curState = States.INTRO;
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
			TransitionToPreRound ();
		}

		void PreRoundUpdate ()
		{
			if (Input.GetKey (KeyCode.Space)) {
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
					TransitionToPreRound ();
				}
			}
		}

		void WinningUpdate ()
		{
		}

		void TransitionToIntro(){
			curState = States.INTRO;
		}

		void TransitionToPreRound(){
			curState = States.PRE_ROUND;
			timeToReset = 0f;
			gameRunning = false;
			hammersManager.Stop ();
			UIMan.setPreRound (true);
			foreach (DustCharecter dust in dusts) {
				dust.resetValues ();
			}
		}

		void TransitionToRound(){
			curState = States.ROUND;
			livingDusts.Clear ();
			livingDusts.AddRange (dusts);
			gameRunning = true;
			foreach (DustCharecter dust in dusts) {
				dust.startRound ();
			}
			hammersManager.Play ();
			UIMan.setPreRound (false);
		}

		void TransitionToWinning(){
			curState = States.WINNING;
		}
	}
}