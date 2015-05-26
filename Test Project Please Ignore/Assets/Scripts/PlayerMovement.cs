using UnityEngine;
using System;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	private CharacterController playerCC;
	public float speed;
	public float gravity;
	public float jumpSpeed;
	new private NetworkView networkView;
	Vector3 moveDirection = Vector3.zero;
	Vector3 groundedMoveDirection = Vector3.zero;

	public NetworkPlayer owner;

	public float validationPeriod = 5;

	void Start () {
		networkView = GetComponent <NetworkView> ();
		playerCC = GetComponent <CharacterController> ();
		if (Network.isServer)
			StartCoroutine (ValidatePosition ());
	}

	IEnumerator ValidatePosition () {
		while (true) {
			networkView.RPC ("ClientReceivePosition", RPCMode.Others, transform.position);
			yield return new WaitForSeconds (validationPeriod);
		}
	}

	public void RPCDestroy () {
		networkView.RPC ("Destroy", RPCMode.AllBuffered);
	}

	[RPC]
	void Destroy () {
		Destroy (gameObject);
	}

	[RPC]
	void SetOwner (NetworkPlayer player) {
		owner = player;
		if (owner == Network.player)
			CameraController.mainCameraCC.SetTarget (transform);
	}

	// Update is called once per frame
	void Update () {
		if (Network.player == owner && Network.isServer) {
			if (playerCC.isGrounded) {
				moveDirection = Vector3.zero;
				if (Input.GetAxis ("Horizontal") != 0)
					moveDirection += Input.GetAxis ("Horizontal") * Vector3.Cross (Vector3.up, CameraController.flatUnitCamToPlayer);
				if (Input.GetAxis ("Vertical") != 0)
					moveDirection += Input.GetAxis ("Vertical") * CameraController.flatUnitCamToPlayer;
				if (moveDirection.magnitude > 1)
					moveDirection.Normalize ();
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
					newMoveDirection += Input.GetAxis ("Vertical") * CameraController.flatUnitCamToPlayer;
				if (newMoveDirection.magnitude > 1)
					newMoveDirection.Normalize ();
				newMoveDirection *= speed;
				moveDirection.x = (groundedMoveDirection.x + newMoveDirection.x) / 2f;
				moveDirection.z = (groundedMoveDirection.z + newMoveDirection.z) / 2f;
			}
			moveDirection.y -= gravity * Time.deltaTime;
			playerCC.Move (moveDirection * Time.deltaTime);
			networkView.RPC ("ClientReceiveMotion", RPCMode.Others, moveDirection * Time.deltaTime);
		} else if (Network.player == owner)
			networkView.RPC ("UpdateClientMotion", RPCMode.Server, new Vector3 (Input.GetAxis ("Horizontal"), Input.GetButton ("Jump") ? 1 : 0, Input.GetAxis ("Vertical")), CameraController.flatUnitCamToPlayer);
	}
	[RPC]
	public void UpdateClientMotion (Vector3 newMotion, Vector3 flatUnitCamToPlayer) {
		if (playerCC.isGrounded) {
			moveDirection = Vector3.zero;
			if (newMotion.x != 0)
				moveDirection += newMotion.x * Vector3.Cross (Vector3.up, flatUnitCamToPlayer);
			if (newMotion.z != 0)
				moveDirection += newMotion.z * flatUnitCamToPlayer;
			if (moveDirection.magnitude > 1)
				moveDirection.Normalize ();
			moveDirection *= speed;
			if (newMotion.y != 0) {
				groundedMoveDirection = moveDirection;
				moveDirection.y = jumpSpeed;
			}
		} else {
			Vector3 newMoveDirection = Vector3.zero;
			if (newMotion.x != 0)
				newMoveDirection += newMotion.x * Vector3.Cross (Vector3.up, flatUnitCamToPlayer);
			if (newMotion.z != 0)
				newMoveDirection += newMotion.z * flatUnitCamToPlayer;
			if (newMoveDirection.magnitude > 1)
				newMoveDirection.Normalize ();
			newMoveDirection *= speed;
			moveDirection.x = (groundedMoveDirection.x + newMoveDirection.x) / 2f;
			moveDirection.z = (groundedMoveDirection.z + newMoveDirection.z) / 2f;
		}
		moveDirection.y -= gravity * Time.deltaTime;
		playerCC.Move (moveDirection * Time.deltaTime);
		networkView.RPC ("ClientReceiveMotion", RPCMode.Others, moveDirection * Time.deltaTime);
	}
	[RPC]
	void ClientReceiveMotion (Vector3 moveDirection) {
		try {
			playerCC.Move (moveDirection);
		} catch (NullReferenceException) {
			playerCC = GetComponent <CharacterController> ();
			playerCC.Move (moveDirection);
		}
	}

	void OnPlayerConnected (NetworkPlayer player) {
		Debug.Log ("Am " + Network.player + " seeing " + player);
		if (Network.isServer)
			networkView.RPC ("ClientReceivePosition", player, transform.position);
	}
	[RPC]
	void ClientReceivePosition (Vector3 position) {
		try {
			playerCC.Move (position - transform.position);
		} catch (NullReferenceException) {
			playerCC = GetComponent <CharacterController> ();
			playerCC.Move (position - transform.position);
		}
	}
}
