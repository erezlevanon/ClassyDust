using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dust
{

	public class DisapearText : Disapear
	{
		public UnityEngine.UI.Text target;

		public float maxAlpha;
		public float minAlpha;
		public float delta;

		public State initState;

		private State curState;
		private State goalState;

		private Color curColor;

		// Use this for initialization
		void Start ()
		{
			curState = initState;
			goalState = initState;
			curColor = target.material.color;
			if (goalState == State.ON) {
				curColor.a = maxAlpha;
			} else {
				curColor.a = minAlpha;
			}
			target.material.color = curColor;
		}


	
		// Update is called once per frame
		void Update ()
		{
			if (curState == goalState)
				return;
			if (goalState == State.ON) {
				curColor.a = Mathf.Min (maxAlpha, curColor.a + delta);
				if (curColor.a == maxAlpha) {
					curState = State.ON;

				}
			} else {
				curColor.a = Mathf.Max (minAlpha, curColor.a - delta);
				if (curColor.a == minAlpha) {
					curState = State.OFF;
				}
			}
			target.material.color = curColor;
		}

		override public void toggle (bool value)
		{
			if (value) {
				goalState = State.ON;
			} else {
				goalState = State.OFF;
			}
		}
	
	}
}