using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {

	void OnServerInitialized () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
	void OnConnectedToServer () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
	void OnDisconnectedFromServer () {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}
}
