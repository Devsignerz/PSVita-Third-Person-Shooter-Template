using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class EnemyAI : MonoBehaviour {
	public float health; //enemy health
	public float timeToDisappear; //amount of time in seconds for enemy body to disappear after death
	public float enemyDamage; //damage done by enemy
	public float distanceToPlayer; //minimum distance needed to hit the player
	public bool hasRagdoll; //does your model have ragdoll
	public GameObject ragdollGO; //the enemy game object with ragdoll enabled
	public GameObject nonRagdollGO; //the enemy game object without ragdoll
	public AudioClip enemyDeathSound; //enemy death sound
	private NavMeshAgent agent; //enemy ai setup

	public void Start() {
		//setup enemy ai for navigation
		agent = GetComponent<NavMeshAgent>();
	}


	public void Death(){
		//disable collision
		GetComponent<CapsuleCollider>().enabled = false;
		//play death sound
		GetComponent<AudioSource>().clip = enemyDeathSound;
		GetComponent<AudioSource>().Play();

		//enable ragdoll
		if(hasRagdoll){
			//if character has ragdoll, replace the mesh with ragdoll
			nonRagdollGO.SetActive(false);
			ragdollGO.SetActive(true);
		}
		Destroy(gameObject, timeToDisappear);
		//DIE
	}

	public void Aware(Transform attnpt){
		//look at player
		transform.LookAt(attnpt.position);

		//follow player
		if(Vector3.Distance(transform.position, attnpt.position) < distanceToPlayer){
			agent.enabled = false;
			Hit(enemyDamage);
		}
		else{
			agent.enabled = true;
			GetComponent<Animator>().ResetTrigger("hit");
			print(Vector3.Distance(transform.position, attnpt.position));
			agent.destination = attnpt.position;
		}
	}
	public void GetHit(float damage){
		health -= damage;
		if(health <= 0){
			Death();
		}
	}
	private void Hit(float damage){
		////hit player with damage
		//enable hit animation
		GetComponent<Animator>().SetTrigger("hit");
		//check for collision with player
	}
}


//editor script for devs
[CustomEditor(typeof(EnemyAI))]
public class MyScriptEditor : Editor{

	override public void OnInspectorGUI(){
		var myScript = target as EnemyAI;
		myScript.health = EditorGUILayout.FloatField("Health", myScript.health);
		myScript.timeToDisappear = EditorGUILayout.FloatField("Time to disappear", myScript.timeToDisappear);
		myScript.enemyDamage = EditorGUILayout.FloatField("Damage", myScript.enemyDamage);
		myScript.distanceToPlayer = EditorGUILayout.FloatField("Minimum distance to attack", myScript.distanceToPlayer);
		myScript.enemyDeathSound = EditorGUILayout.ObjectField("Enemy death sound", myScript.enemyDeathSound, typeof(AudioClip), true) as AudioClip;
		myScript.hasRagdoll = GUILayout.Toggle(myScript.hasRagdoll, "Has ragdoll");
     
		if(myScript.hasRagdoll){
			//myScript.i = EditorGUILayout.IntSlider("I field:", myScript.i , 1 , 100);
			myScript.ragdollGO = EditorGUILayout.ObjectField("Ragdoll GameObject", myScript.ragdollGO, typeof(GameObject), true) as GameObject;
			myScript.nonRagdollGO = EditorGUILayout.ObjectField("Non-Ragdoll GameObject", myScript.nonRagdollGO, typeof(GameObject), true) as GameObject;
		}
	}
}