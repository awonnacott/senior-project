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

	public GameObject shot;
	public Transform shotSpawn;
	private GameController gameController;

	void Start() {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject == null) {
			Debug.Log ("Cannot find 'GameController' script");
		} else {
			gameController = gameControllerObject.GetComponent<GameController> ();
		}
	}

	void Update () {
		if (Input.GetButtonDown ("Fire1")) { // Easier than Input.GetButton ("Fire1") && Time.time > nextFire
			Instantiate (shot, shotSpawn.position, Quaternion.identity);
			gameController.AddScore (-1);
		}
	}

	// FixedUpdate is called once per physics
	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		// Auto-scale mouse axis with scale of rendering window?
		moveHorizontal += Input.GetAxis ("Mouse X");
		moveVertical += Input.GetAxis ("Mouse Y");
		GetComponent<Rigidbody> ().velocity = new Vector3 (moveHorizontal, 0, moveVertical) * speed;

		Vector3 position = GetComponent<Rigidbody>().position;
		GetComponent<Rigidbody> ().position = new Vector3 (
			Mathf.Clamp (position.x, boundary.xMin, boundary.xMax),
			0,
			Mathf.Clamp (position.z, boundary.zMin, boundary.zMax)
		);

		GetComponent<Rigidbody> ().rotation = Quaternion.Euler (Vector3.forward * GetComponent<Rigidbody> ().velocity.x * -tilt); // Should this be +tilt=2 on y? Get particles in line?
	}
}
