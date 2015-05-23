using UnityEngine;
using System.Collections;

public class SphereController : MonoBehaviour, ReactorController {
	new Renderer renderer;
	new NetworkView networkView;
	bool wasReset = false;
	public string textname;
	void Start () {
		renderer = GetComponent <Renderer> ();
		networkView = GetComponent <NetworkView> ();
	}

	public void OnTriggerEnter () {
		Debug.Log ("Hello");
		SetColor(new Vector3 (1, 0, 0));
	}

	public void OnTriggerStay () {
		if (wasReset) {
			SetColor (new Vector3 (1, 0, 0));
			wasReset = false;
			Debug.Log ("Triggered anti-reset on " + textname);
		}
	}

	public void OnTriggerExit () {
		Debug.Log ("Goodbye");
		SetColor (new Vector3 (1, 1, 1));
		wasReset = true;
	}

	[RPC]
	public void SetColor (Vector3 newColorVector) {
		renderer.material.color = new Color(newColorVector.x, newColorVector.y, newColorVector.z);
	}
	

	
	public void Reset () {
		SetColor (new Vector3 (1, 1, 1));
		wasReset = true;
		networkView.RPC ("ResetColor", RPCMode.AllBuffered);
		Debug.Log ("Resetting " + textname);
	}
	[RPC]
	public void ResetColor () {
		SetColor (new Vector3 (1, 1, 1));
		wasReset = true;
	}
}
