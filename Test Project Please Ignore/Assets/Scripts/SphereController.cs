using UnityEngine;
using System.Collections;

public class SphereController : MonoBehaviour {
	new Renderer renderer;
	new NetworkView networkView;
	bool wasReset = false;
	Vector3 defaultColor = Vector3.one;
	void Start () {
		renderer = GetComponent <Renderer> ();
		networkView = GetComponent <NetworkView> ();
	}

	void OnTriggerEnter (Collider other) {
		try {
			Renderer otherRenderer = other.GetComponent <Renderer> ();
			Color otherColor = otherRenderer.material.color;
			SetColor(otherColor);
		} catch (MissingComponentException) {
			SetColor (Vector3.zero);
		}
	}

	void OnTriggerStay (Collider other) {
		if (wasReset) {
			try {
				Renderer otherRenderer = other.GetComponent <Renderer> ();
				Color otherColor = otherRenderer.material.color;
				SetColor(otherColor);
			} catch (MissingComponentException) {
				SetColor (Vector3.zero);
			}
			wasReset = false;
		}
	}

	void OnTriggerExit () {
		SetColor (defaultColor);
		wasReset = true;
	}

	[RPC]
	public void SetColor (Vector3 newColorVector) {
		renderer.material.color = new Color(newColorVector.x, newColorVector.y, newColorVector.z);
	}

	public void SetColor (Color newColor) {
		renderer.material.color = newColor;
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
		SetColor (defaultColor);
		wasReset = true;
		networkView.RPC ("ResetColor", RPCMode.AllBuffered);
	}
	[RPC]
	public void ResetColor () {
		SetColor (defaultColor);
		wasReset = true;
	}

	public void Blue () {
		defaultColor = Vector3.forward;
		ResetColor ();
		wasReset = true;
	}

	public void Green () {
		defaultColor = Vector3.up;
		ResetColor ();
		wasReset = true;
	}

	public void Red () {
		defaultColor = Vector3.right;
		ResetColor ();
		wasReset = true;
	}
}
