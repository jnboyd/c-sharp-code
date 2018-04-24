using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour {

	//How fast the enemy moves
	public float enemySpeed;
	//How close the enemy gets to the player
	public float stoppingDistance;
	//Distance from player where enemy will retreat
	public float retreatDistance;
	//Distance from player enemy will aggro
	public float aggroDistance;
	public int enemyHealth = 1;
	public AudioClip gotHit;

	private float timeBetweenShots;
	public float startTimeBetweenShots;

	public GameObject projectile;
	private Transform player;
	private Animator animator;
	private BoardCreator boardScript;


	void Awake (){

		GameObject g = GameObject.Find ("GameManager"); // find the game manager object

		boardScript = g.GetComponent<BoardCreator> (); // use to it to get a reference to the current boardcreator

	}

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		animator = GetComponent<Animator> ();
		timeBetweenShots = startTimeBetweenShots;
		//animator.SetTrigger ("Enemy2Left");
	}

	void TakeDamage(int damage){
		SoundManager.instance.PlaySingle (gotHit);
		enemyHealth -= damage;
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
		if (Vector2.Distance (transform.position, player.position) < aggroDistance) {
			//Sets direction/animation of enemy
			if (enemyAnimator == 1) {
				animator.SetTrigger ("Enemy2Left");
			} else {
				animator.SetTrigger ("Enemy2Right");
			}
			//Moves enemy towards player
			if (Vector2.Distance (transform.position, player.position) > stoppingDistance) {
				transform.position = Vector2.MoveTowards (transform.position, player.position, enemySpeed * Time.deltaTime);

				//Checks if enemy is near enough to stop moving towards player
			} else if (Vector2.Distance (transform.position, player.position) < stoppingDistance && Vector2.Distance (transform.position, player.position) > retreatDistance) {
				transform.position = this.transform.position;

				//Moves enemy away from player
			} else if (Vector2.Distance (transform.position, player.position) < retreatDistance) {
				transform.position = Vector2.MoveTowards (transform.position, player.position, -enemySpeed * Time.deltaTime);
			}

			if (timeBetweenShots <= 0) {

				Instantiate (projectile, transform.position, Quaternion.identity);
				timeBetweenShots = startTimeBetweenShots;

			} else {
				timeBetweenShots -= Time.deltaTime;
			}
		}
	}

	int enemyDirection(){
		Vector3 direction = (player.transform.position - transform.position).normalized;
		if (direction.x < 0) {
			return 1;
		} else {
			return 2;
		}
	}
}