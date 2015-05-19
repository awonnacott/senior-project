using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {
	public GameObject explosion;
	public int scoreValue;

	void OnTriggerEnter (Collider other) {
		if (other.tag != "Boundary") {
			GameController.AddScore (scoreValue);
			Destroy (other.gameObject);
			if (tag == "Player") {
				GameController.GameOver();
			}
			Destroy (gameObject);
			Instantiate (explosion, transform.position, transform.rotation);
		}
	}
}
