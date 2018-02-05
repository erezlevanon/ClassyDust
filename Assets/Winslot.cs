using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dust
{
	public class Winslot : MonoBehaviour
	{

		private SpriteRenderer spriteRenderer;

		[Header ("images")]
		public Sprite winImage;
		public Sprite loseImage;

		public void Start() {
			spriteRenderer = GetComponent<SpriteRenderer> ();

		}

		public void Set(bool win){
			if (spriteRenderer == null)
				return;
			if (win) {
				spriteRenderer.sprite = winImage;
			} else {
				spriteRenderer.sprite = loseImage;
			}
		}
	}
}