using UnityEngine;

public class PlayerController : MonoBehaviour {
	public CharacterController characterController; //player character controller
	public float speed = 6f; //player move speed
	private float turnSmoothTime = 0.1f; //player rotation smoothness
	private float turnSmoothVelocity; //camera rotation smooth velocity
	public float rotationSpeed; //player rotation speed
	public float health = 100f; //player health
	public bool isArmed = false; //is player aiming
	[SerializeField]private Transform thetarget; //camera point target
	[SerializeField]private Transform thearmedTarget; //camera point target when aiming
	[SerializeField]private GameObject aimsight; //aim dot sight

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
			speed = 3f; 
			Camera.main.GetComponent<CameraController>().target = thearmedTarget;
			aimsight.SetActive(true);
		} 
		// Unarm the player
		else { 
			isArmed = false; 
			speed = 6f;
			Camera.main.GetComponent<CameraController>().target = thetarget; 
			aimsight.SetActive(false);
		} 
		

		// Get left stick input
		float hor = Input.GetAxisRaw("L X");
		float ver = Input.GetAxisRaw("L Y");
		Vector3 dir = new Vector3(hor, 0, ver).normalized;

		//character movement
		if (dir.magnitude >= 0.1f){
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
			// Ensure the player faces the camera's direction even when not moving
			Vector3 cameraForward = Camera.main.transform.forward;
			cameraForward.y = 0;
			// Ensure the character does not tilt up/down
			Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
		}
	}
}
