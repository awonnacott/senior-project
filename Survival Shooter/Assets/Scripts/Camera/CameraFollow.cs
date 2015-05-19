using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform target;
	
	public static Vector3 unitCamToPlayer = new Vector3 ();

	// Isomorphic camera
	/*
	Vector3 offset;
	public float smoothing = 5;
	void Start () {
		offset = transform.position - target.position;
	}
	void FixedUpdate () {
		transform.position = Vector3.Lerp (transform.position, target.position + offset, smoothing * Time.deltaTime);
	}
	*/

		// Perspective camera

	void FixedUpdate () {
		unitCamToPlayer = (target.position - transform.position).normalized;
		Vector3 newAngles = Quaternion.LookRotation (unitCamToPlayer).eulerAngles;
		if (newAngles.y < 180) {
			newAngles.y = Mathf.Clamp (newAngles.y, 0, 20);
		} else if (newAngles.y > 180) {
			newAngles.y = Mathf.Clamp (newAngles.y, 340, 360);
		}
		newAngles.z = 0;
		transform.rotation = Quaternion.Euler (newAngles);
	}
}
