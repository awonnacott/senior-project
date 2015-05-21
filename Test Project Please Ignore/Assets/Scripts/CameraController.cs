using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	Transform target;
	public static Vector3 camToPlayer = Vector3.zero;
	public static Vector3 flatUnitCamToPlayer = Vector3.zero;
	public static CameraController mainCameraCC;

	void Start () {
		mainCameraCC = GameObject.FindWithTag ("MainCamera").GetComponent <CameraController> ();
	}

	public void SetTarget (Transform newTarget) {
		target = newTarget;
	}

	void LateUpdate () {
		if (target != null)
			UpdateRotation ();
	}
	void UpdateRotation () {
		camToPlayer = (target.position - transform.position);
		flatUnitCamToPlayer = new Vector3 (camToPlayer.x, 0, camToPlayer.z).normalized;
		Vector3 newAngles = Quaternion.LookRotation (camToPlayer).eulerAngles;
		newAngles.z = 0;
		transform.rotation = Quaternion.Euler (newAngles);
	}
}
