using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float speed;
	public int projectileDamage;

	private Transform player;
	private Vector2 target;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;		//Player's position
		target = new Vector2 (player.position.x, player.position.y);		//Target position of our projectile
	}
	
	// Update is called once per frame
	void Update () {
		//Projectile will move towards player position when fired (won't track player movement)
		transform.position = Vector2.MoveTowards (transform.position, target, speed * Time.deltaTime);

		if (transform.position.x == target.x && transform.position.y == target.y) {
			DestroyProjectile ();
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Player")) {
			other.gameObject.SendMessage ("TakeDamage", projectileDamage);
			DestroyProjectile ();
		}
	}

	void DestroyProjectile(){
		Destroy (gameObject);
	}
}