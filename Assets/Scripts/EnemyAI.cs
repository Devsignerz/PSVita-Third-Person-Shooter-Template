using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
	public float health;
	public void Death(){
		Destroy(gameObject, 2);
		//DIE
	}

	public void Aware(){
		//look at player
		//follow player
	}
	public void Hit(float damage){
		health -= damage;
		if(health <= 0){
			Death();
		}
		//hit player when gets close to player
	}
}
