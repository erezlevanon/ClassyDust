using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dust
{

	public class UIManager : MonoBehaviour
	{
		[System.Serializable]
		public struct pair {
			public Disapear disapear;
			public bool whenOn;
		}

		public List<pair> Intro;
		public List<pair> preRound;
		public List<pair> Round;
		public List<pair> Winning;

		private Dictionary<GameManager.States, List<pair>> states;

		public void Start(){
			states = new Dictionary<GameManager.States, List<pair>> ();
			states [GameManager.States.INTRO] = Intro;
			states [GameManager.States.PRE_ROUND] = preRound;
			states [GameManager.States.ROUND] = Round;
			states [GameManager.States.WINNING] = Winning;


		}

		public void setState (GameManager.States state, bool value)
		{
			foreach (pair dis in states[state]) {
				if (value) {
					dis.disapear.toggle (dis.whenOn);
				} else {
					dis.disapear.toggle (!(dis.whenOn));
				}
			};
		}
	}
}
