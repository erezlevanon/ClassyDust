using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dust
{
	public class Winslot : MonoBehaviour
	{

		[SerializeField]
		private SpriteRenderer spriteRenderer;

		[Header ("images")]
		public Sprite winImage;
		public Sprite loseImage;

		public void Set(bool win){
			if (spriteRenderer == null)
				spriteRenderer = GetComponent<SpriteRenderer> ();
			if (win) {
				spriteRenderer.sprite = winImage;
			} else {
				spriteRenderer.sprite = loseImage;
			}
		}
	}
}