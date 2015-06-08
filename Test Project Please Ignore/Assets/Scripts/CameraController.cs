using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	Transform target;
	public static Vector3 camToPlayer = Vector3.zero;
	public static Vector3 flatUnitCamToPlayer = Vector3.zero;
	public static CameraController mainCameraCC;

	private Vector3 desiredLocalPosition;
	private float desiredDistance;
	private Quaternion originalRotation;
	private Quaternion parentOriginalRotation;

	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	
	public float maximumX = 360F;

	float rotationX = 0F;
	public float rotationY = 0F;

	float minimumY = -30F;
	float maximumY = 30F;

	void Start () {
		desiredLocalPosition = transform.position;
		desiredDistance = desiredLocalPosition.magnitude;
		originalRotation = transform.localRotation;
		mainCameraCC = GameObject.FindWithTag ("MainCamera").GetComponent <CameraController> ();
	}

	public void SetTarget (Transform newTarget) {
		target = newTarget;
		transform.position = desiredLocalPosition;
		transform.rotation = transform.localRotation;
		transform.SetParent (newTarget);
		if (target != null) {
			transform.localPosition = desiredLocalPosition;
			parentOriginalRotation = transform.parent.localRotation;
		}
	}

	void LateUpdate () {
		if (target != null)
			UpdateRotation ();
	}
	void UpdateRotation () {

		rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
		Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, Vector3.left);
		transform.localRotation = originalRotation * yQuaternion;
		transform.localPosition = yQuaternion * desiredLocalPosition;

		rotationX += Input.GetAxis ("Mouse X") * sensitivityX;
		rotationX = rotationX % maximumX;
		Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
		transform.parent.localRotation = parentOriginalRotation * xQuaternion;

		RaycastHit hit;
		if (Physics.Raycast (transform.parent.position, transform.position - transform.parent.position, out hit, desiredDistance))
			transform.localPosition = transform.localPosition.normalized * Mathf.Clamp (hit.distance, 1.22f, desiredDistance);



		camToPlayer = (target.position - transform.position);
		flatUnitCamToPlayer = new Vector3 (camToPlayer.x, 0, camToPlayer.z).normalized;
		//Vector3 newAngles = Quaternion.LookRotation (camToPlayer).eulerAngles;
		//newAngles.z = 0;
		//transform.rotation = Quaternion.Euler (newAngles);
	}
}
