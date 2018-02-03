using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dust
{

	public class KeyBoardController : DustController
	{

		public KeyCode rightKey;
		public KeyCode leftKey;
		public KeyCode jumpKey;
		public KeyCode pushKey;
	
		public override List<Action> getActions ()
		{
			List<Action> actions = new List<Action>();
			if (Input.GetKey (jumpKey)) {
				actions.Add (Action.JUMP);
			}
			if (Input.GetKey (pushKey)) {
				actions.Add (Action.PUSH);
			}
			if (Input.GetKey (rightKey)) {
				actions.Add (Action.RIGHT);
			} 
			if (Input.GetKey (leftKey)) {
				actions.Add (Action.LEFT);
			}
			return actions;
		}
	}
}