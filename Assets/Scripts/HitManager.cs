using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour {
	[HideInInspector]public float damage;
	[SerializeField] private EnemyAI enemyAIScript;
	void Start(){
		damage = enemyAIScript.enemyDamage;
	}
}
