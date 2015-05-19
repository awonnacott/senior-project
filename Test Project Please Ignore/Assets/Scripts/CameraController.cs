using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public Transform target;
	public static Vector3 camToPlayer = Vector3.zero;
	public static Vector3 flatUnitCamToPlayer = Vector3.zero;
	
	void Update () {
		camToPlayer = (target.position - transform.position);
		flatUnitCamToPlayer = new Vector3 (camToPlayer.x, 0, camToPlayer.z).normalized;
		Vector3 newAngles = Quaternion.LookRotation (camToPlayer).eulerAngles;
		//if (newAngles.y < 180) {
		//	newAngles.y = Mathf.Clamp (newAngles.y, 0, 20);
		//} else if (newAngles.y > 180) {
		//	newAngles.y = Mathf.Clamp (newAngles.y, 340, 360);
		//}
		newAngles.z = 0;
		transform.rotation = Quaternion.Euler (newAngles);
	}
}
