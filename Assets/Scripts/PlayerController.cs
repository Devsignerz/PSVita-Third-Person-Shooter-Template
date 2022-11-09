using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour {
	public CharacterController characterController; //player character controller
	public float speed = 6f; //player move speed
	private float turnSmoothTime = 0.1f; //player rotation smoothness
	private float turnSmoothVelocity; //player rotation speed
	public float health = 100f; //player health
	public float vSpeed = 0f; //player jump speed
	public float gravity = 9.8f; //gravity
	public bool isArmed = false; //is player aiming
	public bool grounded; //is player on the ground
	public bool canJump;
	[SerializeField]private Transform thetarget; //camera point target
	[SerializeField]private Transform thearmedTarget; //camera point target when aiming

	
	public void GetHit(float damage){
		health -= damage;
		if(health < 0){
			//die
			print("DED!");
		}
	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "enemy hand"){
			GetHit(col.gameObject.GetComponent<HitManager>().damage);
		}
	}

	void Update(){

		////DEBUG_make enemy aware
		if(Input.GetKey(KeyCode.X))
			GameObject.FindGameObjectWithTag("enemy").GetComponent<EnemyAI>().Aware(transform);

		//arm the player with weapons
		if(Input.GetKey(KeyCode.JoystickButton4) || Input.GetKey(KeyCode.Q)){
			isArmed = true;
			speed = 3f;
			Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.LookAt = thearmedTarget;
			Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Follow = thearmedTarget;
		}
		//unarm the player
		else{
			isArmed = false;
			speed = 6f;
			Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.LookAt = thetarget;
			Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Follow = thetarget;
			
		}
		//if player can jump in game
		if(canJump){
			//sloppy fix for a bug in game detecting if character is on ground
			if(transform.position.y <= (characterController.skinWidth * -1))
				transform.position = new Vector3(transform.position.x, 0, transform.position.z);
			grounded = characterController.isGrounded;
		}
		//get left and right stick
		float hor = Input.GetAxisRaw("L X");
		float ver = Input.GetAxisRaw("L Y");
		//if player is on ground, stop dropping down
		if (characterController.isGrounded){
			vSpeed = 0;
		}
		vSpeed -= gravity * Time.deltaTime;
		Vector3 dir = new Vector3(hor, 0, ver).normalized;
		
		//handle player turning
		float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + Camera.main.gameObject.transform.eulerAngles.y;
		float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
		
		//if player is armed, turn while camera moves
		if(isArmed){
			transform.rotation = Quaternion.Euler(0f, angle, 0f);
		}
		//else only rotate player when it moves
		else{
			if(dir.magnitude >= 0.1){
				transform.rotation = Quaternion.Euler(0f, angle, 0f);
			}
		}

		//setup move direction for x,y, and z
		Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
		moveDir.y = vSpeed;
		if(dir.magnitude <= 0f){
			moveDir.x = 0;
			moveDir.z = 0;
		}
		//move character
		characterController.Move(moveDir.normalized * speed * Time.deltaTime);
	}
}
