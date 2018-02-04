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

		public List<pair> preRound;

		public void setPreRound (bool value)
		{
			foreach (pair dis in preRound) {
				if (value) {
					dis.disapear.toggle (dis.whenOn);
				} else {
					dis.disapear.toggle (!(dis.whenOn));
				}

			};
		}
	}
}
