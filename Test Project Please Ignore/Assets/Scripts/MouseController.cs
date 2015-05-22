using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {
	public static bool lockCursor = false;
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape))
			lockCursor = false;
		if (lockCursor) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		} else {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}
}
