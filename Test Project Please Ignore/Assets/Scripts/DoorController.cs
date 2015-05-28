using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {

	new Renderer renderer;
	new Collider collider;
	new NetworkView networkView;
	public bool closedByDefaultEh;
	bool closedEh;

	void Start () {
		renderer = GetComponent <Renderer> ();
		collider = GetComponent <Collider> ();
		networkView = GetComponent <NetworkView> ();
		Reset ();
	}

	[RPC]
	public void SetState (bool newClosedEh) {
		renderer.enabled = closedEh;
		collider.enabled = closedEh;
		closedEh = newClosedEh;
	}

	public void ToggleState () {
		SetState (!closedEh);
		networkView.RPC ("SetState", RPCMode.AllBuffered, closedEh); // note that the previous SetState will have toggled closedEh
	}

	public void ToggleStateForSeconds (float stateStayLength) {
		ToggleState ();
		StartCoroutine (WaitThenToggle (stateStayLength));
	}
	
	IEnumerator WaitThenToggle (float waitLength) {
		yield return new WaitForSeconds(waitLength);
		ToggleState();
	}

	void OnServerInitialized() {
		AllReset ();
	}
	void OnPlayerConnected() {
		AllReset ();
	}
	void OnPlayerDisconnected () {
		AllReset ();
	}
	void OnDisconnectedFromServer () {
		Reset ();
	}

	void AllReset () {
		SetState (closedByDefaultEh);
		networkView.RPC ("Reset", RPCMode.AllBuffered);
	}
	[RPC]
	public void Reset () {
		SetState (closedByDefaultEh);
	}
}