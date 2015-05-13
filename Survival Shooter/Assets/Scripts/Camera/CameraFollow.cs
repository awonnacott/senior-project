using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform target;
	public float smoothing = 5;

	Vector3 offset;

	void Start () {
		offset = transform.position - target.position;
	}

	void FixedUpdate () {
		transform.position = Vector3.Lerp (transform.position, target.position + offset, smoothing * Time.deltaTime);
		// Perspective camera?
		//Vector3 camToPlayer = target.position - transform.position;
		//Quaternion newRotation = Quaternion.LookRotation (camToPlayer);
		//transform.rotation = newRotation;
	}
}
