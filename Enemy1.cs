using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour {

	// How much damage the player takes when attacked
	public int attackDamage;
	public float enemySpeed;
	public float enemyDistance;
	public float aggroDistance;
	public float attackDistance;
	public int enemyHealth;
	public Transform transform;
	public AudioClip gotHit;
	public AudioClip attacking;

	private Animator animator;
	private Rigidbody2D rb2d;
	private Transform target;
	private bool isMoving;
	public float fireRate = 0.75F;
	private float nextFire = 0.0F;

	private BoardCreator boardScript;


	void Awake(){
		GameObject g = GameObject.Find ("GameManager"); // find the game manager object

		boardScript = g.GetComponent<BoardCreator> (); // use to it to get a reference to the current boardcreator
	}

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		//animator.SetTrigger ("Enemy1FrontIdle");
		target = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
	}


	void TakeDamage(int damage){
		enemyHealth -= damage;
		SoundManager.instance.PlaySingle (gotHit);
		//rb2d.AddForce (transform.forward);
		CheckDead ();
	}

	void CheckDead(){
		if (enemyHealth <= 0) {
			Destroy (gameObject);
			boardScript.decreaseEnemyCount ();
		}
	}

	// Update is called once per frame
	void Update () {
		int enemyAnimator = enemyDirection ();
		if (Vector2.Distance (transform.position, target.position) < aggroDistance) {		//If Player is within aggroDistance of enemy
			if (Vector2.Distance (transform.position, target.position) > enemyDistance) {	//If Player is further than the distance the enemy stops moving
				transform.position = Vector2.MoveTowards (transform.position, target.position, enemySpeed * Time.deltaTime);	//Move enemy towards player
				if (enemyAnimator == 1) {
					animator.SetTrigger ("Enemy1WalkLeft");
					/*if (Vector2.Distance (transform.position, target.position) <= enemyDistance + 0.5) {
						animator.SetTrigger ("Enemy1AttackLeft");
					}*/
				} else if (enemyAnimator == 2) {
					animator.SetTrigger ("Enemy1WalkDown");
					/*if (Vector2.Distance (transform.position, target.position) <= enemyDistance + 0.5) {
						animator.SetTrigger ("Enemy1AttackDown");
					}*/
				} else if (enemyAnimator == 3) {
					animator.SetTrigger ("Enemy1WalkRight");
					/*if (Vector2.Distance (transform.position, target.position) <= enemyDistance + 0.5) {
						animator.SetTrigger ("Enemy1AttackRight");
					}*/
				} else if (enemyAnimator == 4) {
					animator.SetTrigger ("Enemy1WalkUp");
					/*if (Vector2.Distance (transform.position, target.position) <= enemyDistance + 0.5) {
						animator.SetTrigger ("Enemy1AttackUp");
					}*/
				}
			}
		}
		if(Time.time > nextFire){
			nextFire = Time.time + fireRate;
			Vector2 A = new Vector2 (transform.position.x, transform.position.y);

			Collider2D[] hitPlayer = Physics2D.OverlapCircleAll (A, 1f);
			for (int i = 0; i < hitPlayer.Length; i++) {
				if (hitPlayer[i].tag == "Player") {
					
					hitPlayer[i].SendMessage ("TakeDamage", 1, SendMessageOptions.DontRequireReceiver);
				}
			}
		}



	}
		
	//Current direction enemy is moving in so we can set an animator trigger
	int enemyDirection(){
		Vector3 direction = (target.transform.position - transform.position).normalized;
		if (direction.x < 0 && direction.y < 0) {	//If enemy is moving down and left
			if (direction.x < direction.y) {		//If enemy is moving more left than down, return 1 for "left".
				return 1;
			} else {
				return 2;							//Else enemy is moving more down than left, return 2 for "down"
			}
		} else if (direction.x > 0 && direction.y < 0) {	//If enemy is moving right and down
			if (direction.x < Mathf.Abs (direction.y)) {	//If enemy is moving more down than right
				return 2;							//Return 2 for "down"
			} else {
				return 3;							//Return 3 for "right"
			}
		} else if (direction.x < 0 && direction.y > 0) {	//If enemy is moving left and up
			if (Mathf.Abs (direction.x) < direction.y) {	//If enemy is moving more up than left
				return 4;							//Return 4 for up
			} else {
				return 1;							//Return 1 for left
			}
		} else if (direction.x > 0 && direction.y > 0) {	//If enemy is moving up and right
			if (direction.x < direction.y) {				//If enemy is moving more up than right
				return 4;							//Return 4 for up
			} else {
				return 3;							//Return 3 for right
			}
		}
		return 0;
	}

}