using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dust
{
	public class WinSlotManager : MonoBehaviour
	{

		private List<Winslot> slots;

		public void Start() {
			slots = new List<Winslot> ();
			slots.AddRange (GetComponentsInChildren<Winslot>());
		}

		public void On() {
			this.gameObject.SetActive(true);
		}

		public void Off() {
			this.gameObject.SetActive(false);
		}

		public void SetWins(int numOfWins) {
			if (slots == null)
				return;
			if (numOfWins > slots.Count)
				return;
			for (int i = 0; i < slots.Count; i++) {
				if (i < numOfWins) {
					slots [i].Set(true);
				} else {
					slots[i].Set(false);
				}
			}
		}
	}
}