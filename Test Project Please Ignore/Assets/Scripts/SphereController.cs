using UnityEngine;
using System.Collections;

public class SphereController : MonoBehaviour {
	Renderer renderer;
	NetworkView networkView;
	void Start () {
		renderer = GetComponent <Renderer> ();
		networkView = GetComponent <NetworkView> ();
	}

	public void OnTriggerEnter () {
		Debug.Log ("Hello");
		networkView.RPC ("SetColor", RPCMode.AllBuffered, new Vector3 (1, 0, 0));
	}

	[RPC]
	public void SetColor (Vector3 newColorVector) {
		renderer.material.color = new Color(newColorVector.x, newColorVector.y, newColorVector.z);
	}
}
