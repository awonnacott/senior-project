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
	Rigidbody playerRigidbody;

	public GameObject shot;
	public Transform shotSpawn;

	void Start() {
		playerRigidbody = GetComponent<Rigidbody> ();
	}

	void Update () {
		Screen.lockCursor = true;
		if (Input.GetButtonDown ("Fire1")) { // Easier than Input.GetButton ("Fire1") && Time.time > nextFire
			Instantiate (shot, shotSpawn.position, Quaternion.identity);
			GameController.AddScore (-1);
		}
	}

	// FixedUpdate is called once per physics
	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		// Auto-scale mouse axis with scale of rendering window?
		moveHorizontal += Input.GetAxis ("Mouse X");
		moveVertical += Input.GetAxis ("Mouse Y");
		playerRigidbody.velocity = new Vector3 (moveHorizontal, 0, moveVertical) * speed;

		Vector3 position = playerRigidbody.position;
		playerRigidbody.position = new Vector3 (
			Mathf.Clamp (position.x, boundary.xMin, boundary.xMax),
			0,
			Mathf.Clamp (position.z, boundary.zMin, boundary.zMax)
		);

		playerRigidbody.rotation = Quaternion.Euler (Vector3.forward * playerRigidbody.velocity.x * -tilt); // Should this be +tilt=2 on y? Get particles in line?
	}
}
