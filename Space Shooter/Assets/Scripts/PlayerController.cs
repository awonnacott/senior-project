using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary {
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {
	public float speed;
	public float tilt;
	public Boundary boundary;

	// FixedUpdate is called once per physics
	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		// Auto-scale mouse axis with scale of rendering window?
		//float moveHorizontal = Input.GetAxis ("Mouse X");
		//float moveVertical = Input.GetAxis ("Mouse Y");
		GetComponent<Rigidbody> ().velocity = new Vector3 (moveHorizontal, 0, moveVertical) * speed;
		Vector3 position = GetComponent<Rigidbody>().position;
		GetComponent<Rigidbody> ().position = new Vector3 (
			Mathf.Clamp (position.x, boundary.xMin, boundary.xMax),
			0,
			Mathf.Clamp (position.z, boundary.zMin, boundary.zMax)
		);
		GetComponent<Rigidbody> ().rotation = Quaternion.Euler (0, 0, GetComponent<Rigidbody> ().velocity.x * -tilt); // Should this be +tilt=2 on y? Get particles in line?
	}
}
