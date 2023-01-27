using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour{
    public float speed = .6f;
    public Vector3 targetPosition;
    public float rotateTarget = 0f;
	//env mask
    public LayerMask mask;
	public GameModeScript gms;

    // Start is called before the first frame update
    void Start(){
        targetPosition = transform.position;
		++gms.maxScore;
    }

    // Update is called once per frame
    void Update(){
		////movement////
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

		//yellow debug raycast to walls
        if (Physics.Raycast(ray, out hit, 100f, mask, QueryTriggerInteraction.Ignore))
            Debug.DrawLine(ray.origin, hit.point, Color.yellow);

		//move three units forward
        if (hit.distance >= 3f)
            if (transform.position == targetPosition)
                if (transform.forward == Vector3.back ||
					transform.forward == Vector3.forward ||
					transform.forward == Vector3.left ||
					transform.forward == Vector3.right){

						targetPosition += transform.forward * 3;

                }

		//teleport player to other side if out of bounds
        if (transform.position.x <= -30){
            transform.position += new Vector3(54f, 0f, 0f);
            targetPosition.x += 54f;
        }
            
        if (transform.position.x >= 30){
            transform.position += new Vector3(-54f, 0f, 0f);
            targetPosition.x += -54f;
        }

		////rotation////
		rotateTarget %= 360;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            if (RotateOpenCheck(-transform.right))
                rotateTarget -= 90f;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            if (RotateOpenCheck(transform.right))
                rotateTarget += 90f;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            if (RotateOpenCheck(-transform.forward))
                rotateTarget += 180f;
    }

    private void FixedUpdate(){
		//smooth character movement and rotation
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed);
		transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.Euler(transform.rotation.x, rotateTarget, transform.rotation.z),
            speed);
    }

	//check if player can rotate
	bool RotateOpenCheck(Vector3 Direction){
        Ray ray = new Ray(transform.position, Direction);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 100f, mask, QueryTriggerInteraction.Ignore);
        if (hit.distance <= 3f)
            return false;
        return true;
    }

	void OnTriggerEnter(Collider col){
		print(col.gameObject);
		//restart scene if collided with ghosts
        if (col.CompareTag("ghost")){
            SceneManager.LoadScene("Main");
        }
		//add score if collided with seeds
		if (col.CompareTag("seed")){
            gms.EatingCookieSoundPlay();
            ++gms.seedScore;
            Destroy(col.gameObject);
        }
    }

}