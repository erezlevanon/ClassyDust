using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dust{

public class UIManager : MonoBehaviour {

	public disapear opacityOverlay;



	public void setDark (bool value) {
			opacityOverlay.toggle (value);
	}
}
}
