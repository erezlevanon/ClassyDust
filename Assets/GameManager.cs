using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmfLite;


namespace Dust
{


	public class GameManager : MonoBehaviour
	{

		private enum States {
			INTRO,
			PRE_ROUND,
			ROUND,
			WINNING,
		};

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
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (!gameRunning && Input.GetKey (KeyCode.Space)) {
				StartGame ();
			}
			if (gameRunning) {
				foreach (DustCharecter dust in dusts) {
					if (!dust.IsAlive () && livingDusts.Contains(dust)) {
						livingDusts.Remove (dust);
					}
				}
				if (livingDusts.Count <= 1) {
					EndRound ();
				}

			} else {
				if (timeToReset != 0 && Time.time > timeToReset) {
					ResetValues ();
				}
			}
		}

		void EndRound ()
		{
			gameRunning = false;
			hammersManager.Stop ();
			timeToReset = Time.time + timeAfterWin;
		}

		void StartGame ()
		{
			livingDusts.Clear ();
			livingDusts.AddRange (dusts);
			gameRunning = true;
			foreach (DustCharecter dust in dusts) {
				dust.startRound ();
			}
			hammersManager.Play ();
			UIMan.setPreRound (false);
		}

		void ResetValues ()
		{
			timeToReset = 0f;
			gameRunning = false;
			hammersManager.Stop ();
			UIMan.setPreRound (true);
			foreach (DustCharecter dust in dusts) {
				dust.resetValues ();
			}
		}
	}
}