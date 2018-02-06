using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dust
{
	
	public class DustCharecter : MonoBehaviour
	{

		public DustController controller;

		private Rigidbody2D body;
		private bool isJumping;
		private long isPushing;
		private int facing;
		[SerializeField]
		private int wins;
		private WinSlotManager winManager;

		[Header ("Movement")]
		public float verticalMultiplier;
		public float horizontalMultiplier;
		public float turboMul;
		public float turboCooldown;
		public float pushDuration;

		[Header ("UI")]
		public GameObject uiArrows;

		[Header ("Debug")]
		public bool invulnurable;

		private float lastTurbo;

		private bool alive;
		private bool canjump;
		private bool frozen;

		private Animator animator;
		private List<Animator> clone_animators;
		private SpriteRenderer sprite_renderer;

		public Vector2 initPosition;

		// Use this for initialization
		void Start ()
		{
			body = GetComponent<Rigidbody2D> ();
			clone_animators = new List<Animator> ();
			clone_animators.AddRange(GetComponentsInChildren<Animator> ()); 
			body.simulated = false;
			initPosition = new Vector2 (body.transform.position.x, body.transform.position.y);
			animator = GetComponent<Animator> ();
			sprite_renderer = GetComponent<SpriteRenderer> (); 
			winManager = GetComponentInChildren<WinSlotManager> ();
			resetValues ();
			wins = 0;
		}

		public void resetValues ()
		{
			this.transform.position = initPosition;
			uiArrows.SetActive (true);
			sprite_renderer.sortingOrder = 0;
			body.simulated = false;
			lastTurbo = Time.time;
			facing = Random.value < .5 ? 1 : -1;
			if (facing == -1) {
				sprite_renderer.flipX = true;
			}
			alive = true;
			canjump = false;
			lastTurbo = Time.time - pushDuration - 1f;
			body.gravityScale = 15f;
			GetComponent<Collider2D> ().enabled = true;
			GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);
			winManager.SetWins (wins);
			winManager.On ();
		}

		public void resetWins(){
			wins = 0;
			winManager.SetWins (wins);
		}

		public void startRound ()
		{
			winManager.On ();
			body.velocity = new Vector2 ();
			body.simulated = true;
			sprite_renderer.sortingOrder = 1;
			uiArrows.SetActive (false);
			winManager.Off ();
		}

		public bool IsPushing ()
		{
			return Time.time - lastTurbo < pushDuration;
		}
			
		// Update is called once per frame
		void FixedUpdate ()
		{
			if (!IsPushing ()) {
				body.mass = 1f;
			}
			if (!alive)
				return;
			Vector2 velocity = body.velocity;
			List<Action> actions = controller.getActions ();
			if (actions.Count != 0 && !body.simulated) {
				if (!frozen) {
					startRound ();
					return;
				} else {
					body.simulated = true;
					return;
				}
			}
			if (actions.Contains (Action.JUMP) && canjump) {
				velocity.y = verticalMultiplier;
				triggerAnimation ("Jump");
				canjump = false;
			}
			if (actions.Contains (Action.PUSH)) {
				if (Time.time - lastTurbo > turboCooldown) {
					velocity.x = facing * turboMul;
					lastTurbo = Time.time;
					body.mass = 5f;
					triggerAnimation ("Turbo");
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

		void OnCollisionEnter2D (Collision2D col)
		{
			if (col.gameObject.tag == "hammer") {
				die ();
			} else if (col.gameObject.tag == "Ground") {
				triggerAnimation ("Land");
			}
			canjump = true;
		}

		void OnCollisionStay2D (Collision2D col)
		{
			DustCharecter otherdust = col.gameObject.GetComponent<DustCharecter> ();
			if (otherdust != null) {
				if (otherdust.IsPushing ()) {
					triggerAnimation ("Pushed");
				}
			}
		}

		void OnTriggerEnter2D (Collider2D col)
		{
			if (col.gameObject.tag == "Teleport") {
				body.transform.position = new Vector2 (Mathf.Sign (body.transform.position.x) * (-1f) * 19f + body.transform.position.x, body.transform.position.y);
			}
		}

		public bool IsAlive ()
		{
			return alive;
		}

		public void die ()
		{
			if (invulnurable)
				return;
			body.velocity = new Vector2 (0f, 0f);
			body.gravityScale = 0.7f;
			GetComponent<Collider2D> ().enabled = false;
			GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0.3f);
			triggerAnimation ("Hit");
			alive = false;
		}

		public void winRound ()
		{
			wins += 1;
		}

		public int getWins ()
		{
			return wins;
		}

		public void showArrows(bool val){
			uiArrows.SetActive (val);
		}
		public void showWins(bool val){
			if (val) {
				winManager.SetWins (wins);
				winManager.On ();
			} else {
				winManager.Off ();
			}
		}

		public bool isMoving() {
			return body.simulated;
		}

		private void triggerAnimation(string name) {
			animator.SetTrigger (name);
			foreach (Animator a in clone_animators) {
				a.SetTrigger (name);
			}
		}

		public void freeze(){
			this.body.constraints = this.body.constraints | RigidbodyConstraints2D.FreezePositionX;
			frozen = true;
		}

		public void unfreeze() {
			this.body.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
			frozen = false;
		}
			
	}
}