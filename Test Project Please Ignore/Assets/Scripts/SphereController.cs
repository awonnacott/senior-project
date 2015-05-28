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
		SetColor (Vector3.right);
	}

	void OnTriggerStay () {
		if (wasReset) {
			SetColor (Vector3.right);
			wasReset = false;
		}
	}

	void OnTriggerExit () {
		SetColor (Vector3.one);
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
		SetColor (Vector3.one);
		wasReset = true;
		networkView.RPC ("ResetColor", RPCMode.AllBuffered);
	}
	[RPC]
	public void ResetColor () {
		SetColor (Vector3.one);
		wasReset = true;
	}

	public void Blue () {
		SetColor (Vector3.forward);
	}

	public void Green () {
		SetColor (Vector3.up);
	}

	public void Red () {
		SetColor (Vector3.right);
	}
}
