using UnityEngine;
using System;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour {
	void OnTriggerExit (Collider other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.transform.position = Vector3.zero;
		} else {
			try {
				DestroyByContact oDBC = other.GetComponent <DestroyByContact> ();
				Debug.Log (oDBC.scoreValue/-3);
				GameController.AddScore(oDBC.scoreValue/-3);
			} catch (NullReferenceException nre) {
			} finally {
				Destroy (other.gameObject);
			}
		}
	}
}
