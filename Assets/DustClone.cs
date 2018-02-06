using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dust
{

	public class DustClone : MonoBehaviour
	{

		private SpriteRenderer spriteRenderer;
		private SpriteRenderer parentSpriteRenderer;

		// Use this for initialization
		void Start ()
		{
			spriteRenderer = GetComponent<SpriteRenderer> ();
			parentSpriteRenderer = GetComponentInParent<SpriteRenderer> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (spriteRenderer == null)
				return;
			spriteRenderer.sprite = parentSpriteRenderer.sprite;
			spriteRenderer.flipX = parentSpriteRenderer.flipX;
			spriteRenderer.color = parentSpriteRenderer.color;
		}
	}
}