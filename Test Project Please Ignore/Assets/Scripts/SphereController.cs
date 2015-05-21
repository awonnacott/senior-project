using UnityEngine;
using System.Collections;

public class SphereController : MonoBehaviour {
	new Renderer renderer;
	new NetworkView networkView;
	void Start () {
		renderer = GetComponent <Renderer> ();
		networkView = GetComponent <NetworkView> ();
	}

	public void OnTriggerEnter () {
		Debug.Log ("Hello");
		networkView.RPC ("SetColor", RPCMode.AllBuffered, new Vector3 (1, 0, 0));
	}

	public void OnTriggerExit () {
		Debug.Log ("Goodbye");
		networkView.RPC ("SetColor", RPCMode.AllBuffered, new Vector3 (1, 1, 1));
	}

	[RPC]
	public void SetColor (Vector3 newColorVector) {
		renderer.material.color = new Color(newColorVector.x, newColorVector.y, newColorVector.z);
	}
}
