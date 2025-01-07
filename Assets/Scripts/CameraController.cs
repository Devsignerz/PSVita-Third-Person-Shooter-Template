using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public Transform target; //camera's look target
	public Vector3 offset; //camera position offset from player
	public float rotationSpeed = 5.0f; //camera rotation speed
	private float yaw;
	private float pitch;

	private void LateUpdate() {
		//get right stick
		float hor = Input.GetAxisRaw("R X");
		float ver = Input.GetAxisRaw("R Y");

		// Rotate the camera based on the right stick input
		yaw += hor * rotationSpeed;
		pitch -= ver * rotationSpeed;
		pitch = Mathf.Clamp(pitch, -45f, 45f); // Limit pitch to avoid extreme angles
		transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
		// Calculate camera rotation
		Quaternion cameraRotation = Quaternion.Euler(pitch, yaw, 0.0f);
		// Make the camera follow the player's position
		transform.position = target.position + cameraRotation * offset;
		transform.LookAt(target);
	}
}
