using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dust
{

	public abstract class Disapear : MonoBehaviour
	{
		public enum State
		{
			ON,
			OFF,
		}



			
		public abstract void toggle (bool value);
	
	}
}