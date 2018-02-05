using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dust {
	
public class DustCharecter : MonoBehaviour {

	public DustController controller;

		private Rigidbody2D body;
		private bool isJumping;
		private long  isPushing;
		private int facing;
		private int wins;

		[Header("Movement")]
		public float verticalMultiplier;
		public float horizontalMultiplier;
		public float turboMul;
		public float turboCooldown;
		public float pushDuration;

		[Header ("UI")]
		public GameObject uiArrows;

		[Header("Debug")]
		public bool invulnurable;

		private float lastTurbo;

		private bool alive;
		private bool canjump;

		private Animator animator;
		private SpriteRenderer sprite_renderer;

		public Vector2 initPosition;

	// Use this for initialization
	void Start () {
			body = GetComponent<Rigidbody2D> ();
			body.simulated = false;
			initPosition = new Vector2(body.transform.position.x, body.transform.position.y);
			animator = GetComponent<Animator> ();
			sprite_renderer = GetComponent<SpriteRenderer> (); 
			resetValues ();
			wins = 0;
	}

		public void resetValues()
		{
			this.transform.position = initPosition;
			uiArrows.SetActive(true);
			sprite_renderer.sortingOrder = 0;
			body.simulated = false;
			lastTurbo = Time.time;
			facing = Random.value < .5? 1 : -1;
			if (facing == -1) {
				sprite_renderer.flipX = true;
			}
			alive = true;
			wins = 0;
			canjump = false;
			lastTurbo = Time.time - pushDuration - 1f;
			body.gravityScale = 15f;
			GetComponent<Collider2D> ().enabled = true;
			GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);
		}

	public void startRound() {
			body.velocity = new Vector2 ();
			body.simulated = true;
			sprite_renderer.sortingOrder = 1;
			uiArrows.SetActive(false);
	}

	public bool IsPushing() {
			return Time.time - lastTurbo < pushDuration;
	}
			
	// Update is called once per frame
	void FixedUpdate () {
			if (!IsPushing ()) {
				body.mass = 1f;
			}
			if (!alive)
				return;
			Vector2 velocity = body.velocity;
			List<Action> actions = controller.getActions ();
			if (actions.Count != 0) {
				startRound ();
			}
			if (actions.Contains(Action.JUMP) && canjump) {
				velocity.y = verticalMultiplier;
				animator.SetTrigger ("Jump");
				canjump = false;
			}
			if (actions.Contains (Action.PUSH)) {
				if (Time.time - lastTurbo > turboCooldown) {
					velocity.x = facing * turboMul;
					lastTurbo = Time.time;
					body.mass = 5f;
					animator.SetTrigger ("Turbo");
				}
				} else {
					if (actions.Contains (Action.RIGHT)) {
						facing = 1;
						velocity.x = horizontalMultiplier;
					} else if (actions.Contains (Action.LEFT)) {
						velocity.x = -horizontalMultiplier;
						facing = -1;
					} 
				}
				body.velocity = velocity;
			if (facing == -1) {
				sprite_renderer.flipX = true;
			} else {
				sprite_renderer.flipX = false;
			}
	}

		void OnCollisionEnter2D (Collision2D col) {
			if (col.gameObject.tag == "hammer") {
				die ();
			} else if (col.gameObject.tag == "Ground") {
				animator.SetTrigger ("Land");
			}
			canjump = true;
		}

		void OnCollisionStay2D (Collision2D col) {
			DustCharecter otherdust = col.gameObject.GetComponent<DustCharecter> ();
			if (otherdust != null) {
				if (otherdust.IsPushing ()) {
					animator.SetTrigger ("Pushed");
				}
			}
		}

		void OnTriggerEnter2D(Collider2D col) {
			if (col.gameObject.tag == "Teleport") {
				body.transform.position = new Vector2 (Mathf.Sign(body.transform.position.x) * (-1f) * 23f + body.transform.position.x , body.transform.position.y);
			}
		}

		public bool IsAlive() {
			return alive;
		}

		public void die() {
			if (invulnurable) return;
			body.velocity = new Vector2 (0f, 0f);
			body.gravityScale = 0.7f;
			GetComponent<Collider2D> ().enabled = false;
			GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0.3f);
			animator.SetTrigger ("Hit");
			alive = false;
		}

		public void winRound(){
			wins += 1;
		}

		public int getWins(){
			return wins;
		}

		public void wakeUp(){
			
		}
			
}
}