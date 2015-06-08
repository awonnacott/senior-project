using UnityEngine;
using System.Collections;

public class CoinController : MonoBehaviour {
	static int targets = 0;
	new Rigidbody rigidbody;
	new NetworkView networkView;
	// Use this for initialization
	void Start () {
		targets += 1;
		rigidbody = gameObject.GetComponent <Rigidbody> ();
		rigidbody.angularVelocity = Random.value * 5 * Vector3.up;
		networkView = gameObject.GetComponent <NetworkView> ();
	}

	void GameOver () {
		Debug.Log ("Win");
	}

	[RPC]
	void Deactivate () {
		gameObject.SetActive (false);
	}

	// Update is called once per frame
	void OnTriggerEnter () {
		if (Network.isServer) 
			networkView.RPC ("Deactivate", RPCMode.AllBuffered);
		targets -= 1;
		if (targets == 0)
			GameOver ();
		Deactivate ();
	}
}
