using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunManager : MonoBehaviour {
	public float damage;
	public string gName;
	public bool singleShot;
	public int ammo;
	public int mag;
	public int currentAmmo;
	[SerializeField] private Text ammoCounter;
	[SerializeField] private PlayerController controller;
	[SerializeField] private Transform gunHole;
	public AudioClip shootingSound;
	public AudioClip reloadSound;

	private void Update(){
		//check if gun is automatic or single shot
		if(singleShot){
			if(controller.isArmed){
				if(Input.GetKeyDown(KeyCode.JoystickButton5) || Input.GetKeyDown(KeyCode.E)){
					shoot();
					GetComponent<AudioSource>().Play();
					ammoCounter.text = currentAmmo +"/"+mag;
				}
			}
			else{
				if(Input.GetKeyDown(KeyCode.JoystickButton5) || Input.GetKeyDown(KeyCode.E)){
					reload();
					GetComponent<AudioSource>().Play();
					ammoCounter.text = currentAmmo +"/"+mag;
				}
			}
		}
	}

	//handle shooting bs
	public void shoot(){
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)){
			print(hit.collider.name);
			Debug.DrawRay(ray.origin, ray.direction, Color.yellow);
			currentAmmo--;
			if(currentAmmo < 0)
				reload();
			else{
				if(hit.collider.tag == "enemy"){
					hit.collider.gameObject.GetComponent<EnemyAI>().Hit(damage);
					print(hit.collider.gameObject.GetComponent<EnemyAI>().health);
				}
				GetComponent<AudioSource>().clip = shootingSound;
			}
		}
		

		
	}

	public void reload(){
		GetComponent<AudioSource>().clip = reloadSound;
		currentAmmo = ammo;
		mag--;
	}
}
