using UnityEngine;

public class PlayerController : MonoBehaviour {
	public CharacterController characterController; //player character controller
	public float speed; //player move speed
	private float turnSmoothTime = 0.1f; //player rotation smoothness
	private float turnSmoothVelocity; //camera rotation smooth velocity
	public float rotationSpeed; //player rotation speed
	public float health; //player health
	public bool isArmed = false; //is player aiming
	[SerializeField]private Transform thetarget; //camera point target
	[SerializeField]private GameObject aimsight; //aim dot sight
	[SerializeField]private Animator playerAnim; //animator attached to player character
	[SerializeField]private Transform playerSpine;

	//take damage
	public void GetHit(float damage){
		health -= damage;
		if(health < 0){
			//die
			print("DED!");
		}
	}

	

	void Update(){

		////DEBUG_make enemy aware
		if(Input.GetKey(KeyCode.X))
			GameObject.FindGameObjectWithTag("enemy").GetComponent<EnemyAI>().Aware(transform);


		// Arm the player with weapons
		if (Input.GetKey(KeyCode.JoystickButton4)){ 
			isArmed = true;
			Camera.main.fieldOfView = 25f;
			aimsight.SetActive(true);
			playerAnim.SetBool("aiming", true);
		} 
		// Unarm the player
		else { 
			isArmed = false; 
			Camera.main.fieldOfView = 40f;
			aimsight.SetActive(false);
			playerAnim.SetBool("aiming", false);
		} 
		

		// Get left stick input
		float hor = Input.GetAxisRaw("L X");
		float ver = Input.GetAxisRaw("L Y");
		Vector3 dir = new Vector3(hor, 0, ver).normalized;

		//character movement
		if (dir.magnitude >= 0.1f){
			playerAnim.SetBool("walking", true);
			if (isArmed){
				// Handle strafing and backward movement when armed
				Vector3 cameraRight = Camera.main.transform.right;
				Vector3 cameraForward = Camera.main.transform.forward;
				cameraForward.y = 0;
				cameraRight.y = 0;
				Vector3 moveDirection = (cameraForward * ver + cameraRight * hor).normalized;
				characterController.Move(moveDirection * speed * Time.deltaTime);

				// Make the player face the camera's direction when armed
				Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
				playerSpine.rotation = Quaternion.Slerp(playerSpine.rotation, Quaternion.LookRotation(Camera.main.transform.forward), rotationSpeed*Time.deltaTime);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
				} 
			else {
				// Handle player turning when not armed
				float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + Camera.main.gameObject.transform.eulerAngles.y;
				float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
				transform.rotation = Quaternion.Euler(0, angle, 0);

				// Move the character
				Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
				characterController.Move(moveDir * speed * Time.deltaTime);
			}
		}
		//camera fix for armed character
		else if(isArmed){
			playerAnim.SetBool("walking", false);
			// Ensure the player faces the camera's direction even when not moving
			Vector3 cameraForward = Camera.main.transform.forward;
			cameraForward.y = 0;
			// Ensure the character does not tilt up/down
			Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
			playerSpine.rotation = Quaternion.Slerp(playerSpine.rotation, Quaternion.LookRotation(Camera.main.transform.forward), rotationSpeed*Time.deltaTime);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
		}
		else{
			playerAnim.SetBool("walking", false);
		}
	}
}
