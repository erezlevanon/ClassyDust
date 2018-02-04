using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dust
{
	public abstract class State : MonoBehaviour
	{

		public enum States
		{
			INTRO,
			PRE_ROUND,
			ROUND,
			WINNING,
		};

		[System.Serializable]
		public struct UIDisapear {
			public Disapear disapear;
			public bool whenOn;
		}

		public List<UIDisapear> UIChanges;

		public States state;

		abstract public void enter ();

		abstract public void leave ();

	}
}