using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {
	public GameObject explosion;
	public int scoreValue;
	private GameController gameController;

	void Start() {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject == null) {
			Debug.Log ("Cannot find 'GameController' script");
		} else {
			gameController = gameControllerObject.GetComponent<GameController> ();
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag != "Boundary") {
			gameController.AddScore (scoreValue);
			Destroy (other.gameObject);
			if (tag == "Player") {
				gameController.GameOver();
			}
			Destroy (gameObject);
			Instantiate (explosion, transform.position, transform.rotation);
		}
	}
}
