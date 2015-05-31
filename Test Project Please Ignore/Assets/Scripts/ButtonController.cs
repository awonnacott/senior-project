using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ButtonController : MonoBehaviour {
	new NetworkView networkView;
	public UnityEvent actions;

	void Start () {
		networkView = GetComponent <NetworkView> ();
	}
	
	void OnTriggerEnter (Collider other) {
		if (Network.isServer) {
			NetworkView otherNV = other.gameObject.GetComponent <NetworkView> ();
			PlayerMovement otherPM = other.gameObject.GetComponent <PlayerMovement> ();
			if (otherNV == null || otherPM == null)
				return;
			else if (otherPM.owner != Network.player)
				otherNV.RPC ("GetButton", otherPM.owner, "Use", networkView.viewID, "ActivateButton");
			else if (otherPM.owner == Network.player && Input.GetButton ("Use"))
				ActivateButton ();
		}
	}
	
	void OnTriggerStay (Collider other) {
		if (Network.isServer) {
			NetworkView otherNV = other.gameObject.GetComponent <NetworkView> ();
			PlayerMovement otherPM = other.gameObject.GetComponent <PlayerMovement> ();
			if (otherNV == null || otherPM == null)
				return;
			else if (otherPM.owner != Network.player)
				otherNV.RPC ("GetButtonDown", otherPM.owner, "Use", networkView.viewID, "ActivateButton");
			else if (otherPM.owner == Network.player && Input.GetButtonDown ("Use"))
				ActivateButton ();
		}
	}

	[RPC]
	public void ActivateButton () {
		if (Network.isServer)
			networkView.RPC ("ActivateButton", RPCMode.OthersBuffered);
		actions.Invoke ();

	}
}
