using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrechBg : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		if (sr == null) return;

		Vector3 res_scale = new Vector3 (1, 1, 1);
	
		float width = sr.sprite.bounds.size.x;
		float height = sr.sprite.bounds.size.y;

		float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;



		res_scale.x = worldScreenWidth / width;
		res_scale.y = worldScreenHeight / height;
		transform.localScale = res_scale;
	}
}
