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

		public List<UnityEngine.UI.Text> rounds;

		[Header("Tweaking")]
		public Color roundOpcaityOn;
		public Color roundOpacityOff;

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

		public void setRound(int round) {
			if (round > 5 || round < 0)
				return;
			for(int i = 1; i <= rounds.Count ; i++){
				if (i == round) {
					rounds [i-1].color = roundOpcaityOn;
				} else {
					rounds [i-1].color = roundOpacityOff;
				}
			}
		}
	}
}
