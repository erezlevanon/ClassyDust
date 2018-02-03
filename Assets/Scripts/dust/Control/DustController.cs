using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dust
{
	
	public enum Action
	{
		RIGHT,
		LEFT,
		JUMP,
		PUSH,
	}

	public abstract class DustController : MonoBehaviour
	{
		/// <summary>
		/// Get the Action to preform at this time.
		/// </summary>
		/// <returns>The action.</returns>
		public abstract List<Action> getActions ();
		
	}
}