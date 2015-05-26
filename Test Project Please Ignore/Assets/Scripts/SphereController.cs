using UnityEngine;
using System.Collections;

public class SphereController : MonoBehaviour {
	new Renderer renderer;
	new NetworkView networkView;
	bool wasReset = false;
	void Start () {
		renderer = GetComponent <Renderer> ();
		networkView = GetComponent <NetworkView> ();
	}

	void OnTriggerEnter () {
		SetColor(new Vector3 (1, 0, 0));
	}

	void OnTriggerStay () {
		if (wasReset) {
			SetColor (new Vector3 (1, 0, 0));
			wasReset = false;
		}
	}

	void OnTriggerExit () {
		SetColor (new Vector3 (1, 1, 1));
		wasReset = true;
	}

	[RPC]
	public void SetColor (Vector3 newColorVector) {
		renderer.material.color = new Color(newColorVector.x, newColorVector.y, newColorVector.z);
	}

	void OnServerInitialized() {
		AllResetColor ();
	}
	void OnPlayerConnected() {
		AllResetColor ();
	}
	void OnPlayerDisconnected () {
		AllResetColor ();
	}
	void OnDisconnectedFromServer () {
		ResetColor ();
	}

	void AllResetColor () {
		SetColor (new Vector3 (1, 1, 1));
		wasReset = true;
		networkView.RPC ("ResetColor", RPCMode.AllBuffered);
	}
	[RPC]
	public void ResetColor () {
		SetColor (new Vector3 (1, 1, 1));
		wasReset = true;
	}
}
