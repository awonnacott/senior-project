using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	private CharacterController playerCC;
	public float speed;
	public float gravity;
	public float jumpSpeed;
	private NetworkView networkView;
	Vector3 moveDirection = Vector3.zero;
	Vector3 groundedMoveDirection = Vector3.zero;

	void Start () {
		networkView = GetComponent <NetworkView> ();
		playerCC = GetComponent <CharacterController> ();
		if (networkView.isMine) {
			CameraController cameraCC = GameObject.FindWithTag ("MainCamera").GetComponent <CameraController> ();
			cameraCC.SetTarget (transform);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (networkView.isMine) {
			if (playerCC.isGrounded) {
				moveDirection = Vector3.zero;
				if (Input.GetAxis ("Horizontal") != 0)
					moveDirection += Input.GetAxis ("Horizontal") * Vector3.Cross (Vector3.up, CameraController.flatUnitCamToPlayer);
				if (Input.GetAxis ("Vertical") != 0)
					moveDirection += Input.GetAxis("Vertical") * CameraController.flatUnitCamToPlayer;
				if (moveDirection.magnitude > 1)
					moveDirection.Normalize();
				moveDirection *= speed;
				if (Input.GetButton ("Jump")) {
					groundedMoveDirection = moveDirection;
					moveDirection.y = jumpSpeed;
				}
			} else {
				Vector3 newMoveDirection = Vector3.zero;
				if (Input.GetAxis ("Horizontal") != 0)
					newMoveDirection += Input.GetAxis ("Horizontal") * Vector3.Cross (Vector3.up, CameraController.flatUnitCamToPlayer);
				if (Input.GetAxis ("Vertical") != 0)
					newMoveDirection += Input.GetAxis("Vertical") * CameraController.flatUnitCamToPlayer;
				if (newMoveDirection.magnitude > 1)
					newMoveDirection.Normalize();
				newMoveDirection *= speed;
				moveDirection.x = (groundedMoveDirection.x + newMoveDirection.x) / 2f;
				moveDirection.z = (groundedMoveDirection.z + newMoveDirection.z) / 2f;
			}
			moveDirection.y -= gravity * Time.deltaTime;
			playerCC.Move (moveDirection * Time.deltaTime);
		} else 
			enabled = false;
	}
}
