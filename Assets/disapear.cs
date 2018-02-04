using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dust
{

	public class disapear : MonoBehaviour
	{


		public enum State
		{
			ON,
			OFF,
		}

		public SpriteRenderer sprite;

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
			curColor = sprite.color;
			if (goalState == State.ON) {
				curColor.a = maxAlpha;
			} else {
				curColor.a = minAlpha;
			}
			sprite.color = curColor;
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
			sprite.color = curColor;
		}

		public void toggle (bool value)
		{
			if (value) {
				goalState = State.ON;
			} else {
				goalState = State.OFF;
			}
		}
	
	}
}