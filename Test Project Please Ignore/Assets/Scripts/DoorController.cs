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
		closedEh = newClosedEh;
		renderer.enabled = closedEh;
		collider.enabled = closedEh;
	}

	public void ToggleState () {
		bool wasClosedEh = closedEh;
		SetState (!wasClosedEh);
		networkView.RPC ("SetState", RPCMode.AllBuffered, !wasClosedEh); // note that the previous SetState will have toggled closedEh but not wasClosedEh
	}

	public void CloseForSeconds (float stateStayLength) {
		SetState (true);
		StartCoroutine (WaitThenToggle (stateStayLength));
	}

	public void OpenForSeconds (float stateStayLength) {
		SetState (false);
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