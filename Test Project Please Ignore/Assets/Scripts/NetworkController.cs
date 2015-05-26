using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class NetworkController : MonoBehaviour {
	public static string gameType = "Test Project Please Ignore";
	public static uint port = 10768;
	
	public string gameName = "Test Server Please Ignore";
	public string password = "867-5309";
	public string comment = "";

	public GameObject playerObject;

	public Button hostButton;
	public Button clientButton;
	public Button disconnectButton;
	public GameObject serversPanel;
	public Transform contentPanel;
	public GameObject serverButton;

	void HostButtonClick () {
		Debug.Log ("Starting Server");
		Network.incomingPassword = password;
		bool useNat = !Network.HavePublicAddress();
		Network.InitializeServer(32, (int)port, useNat);
	}
	void OnServerInitialized () {
		Debug.Log ("Server Initialized");
		SpawnPlayer (Network.player);
		serversPanel.SetActive (false);
		hostButton.interactable = false;
		clientButton.interactable = false;
		disconnectButton.interactable = true;
		Debug.Log ("Registering Server with Master Server");
		MasterServer.RegisterHost(gameType, gameName, comment);
	}
	void OnMasterServerEvent (MasterServerEvent msEvent) {
		switch (msEvent) {
		case MasterServerEvent.HostListReceived:
			Debug.Log ("Hosts Refreshed");
			OnHostsRefreshed ();
			break;
		case MasterServerEvent.RegistrationFailedGameName:
			Debug.Log ("Master Server Registration Failed: game name");
			break;
		case MasterServerEvent.RegistrationFailedGameType:
			Debug.Log ("Master Server Registration Failed: game type");
			break;
		case MasterServerEvent.RegistrationFailedNoServer:
			Debug.Log ("Master Server Registration Failed: no server");
			break;
		case MasterServerEvent.RegistrationSucceeded:
			Debug.Log ("Master Server Registration Success");
			break;
		default:
			Debug.Log ("Unknown Master Server Event: " + msEvent);
			break;
		}
	}
	void OnFailedToConnectToMasterServer (NetworkConnectionError info) {
		Debug.Log ("Master Server Connection Failed: " + info);
	}
	
	void ClientButtonClick () {
		if (serversPanel.activeSelf)
			serversPanel.SetActive (false);
		else {
			Debug.Log ("Refreshing Hosts");
			MasterServer.ClearHostList ();
			MasterServer.RequestHostList (gameType);
		}
	}
	void OnHostsRefreshed () {
		HostData[] hosts = MasterServer.PollHostList ();
		PopulateHostList (hosts);
	}
	void PopulateHostList (HostData[] hosts) {
		serversPanel.SetActive (true);
		foreach (Transform child in contentPanel)
			GameObject.Destroy (child.gameObject);
		//Debug.Log (hosts.Length + " games of Game Type " + gameType);
		foreach (HostData host in hosts) {
			/*Debug.Log (host.gameName + " (passwordProtected: " + host.passwordProtected + ")"
				+ "\n\tplayers: " + host.connectedPlayers + " / " + host.playerLimit
				+ "\n\tip: " + host.ip + ":" + host.port
				+ "\n\tuseNAT: " + host.useNat + " GUID: " + host.guid
				+ "\n\tdescription: " + host.comment);
				*/
			GameObject newButton = Instantiate(serverButton) as GameObject;
			ServerButtonController serverButtonController = newButton.GetComponent <ServerButtonController> ();
			serverButtonController.AssignHost(this, host);
			newButton.transform.SetParent (contentPanel);
		}
	}
	public void ConnectToHost (HostData host) {
		Debug.Log ("Connecting to " + host.gameName);
		NetworkConnectionError nce = Network.Connect (host, password);
		Debug.Log (nce);
	}
	void OnConnectedToServer () {
		Debug.Log ("Connected");
		serversPanel.SetActive (false);
		hostButton.interactable = false;
		clientButton.interactable = false;
		disconnectButton.interactable = true;
	}
	void OnPlayerConnected (NetworkPlayer player) {
		SpawnPlayer (player);
	}
	void SpawnPlayer (NetworkPlayer player) {
		Vector3 spawnPosition = new Vector3 (Random.value * 10 - 5, 0, Random.value * 10 - 5);
		GameObject newPlayerObject = (GameObject) Network.Instantiate (playerObject, spawnPosition, Quaternion.identity, 0);
		newPlayerObject.GetComponent <NetworkView> ().RPC("SetOwner", RPCMode.AllBuffered, player);
		newPlayerObject.layer = LayerMask.NameToLayer ("Ignore Raycast");
	}

	public void DisconnectButtonClick () {
		if (Network.isServer) {
			Network.Disconnect ();
			MasterServer.UnregisterHost ();
		} else if (Network.isClient)
			Network.Disconnect ();
	}
	void OnDisconnectedFromServer(NetworkDisconnection info) {
		if (Network.isServer)
			Debug.Log("Local server connection disconnected");
		else if (info == NetworkDisconnection.LostConnection)
			Debug.Log("Lost connection to the server");
		else
			Debug.Log("Successfully diconnected from the server");
		hostButton.interactable = true;
		clientButton.interactable = true;
		disconnectButton.interactable = false;
		CameraController.mainCameraCC.SetTarget (null);
		GameObject[] playerObjects = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject playerObject in playerObjects)
			Destroy(playerObject);
	}
	void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("Clean up after player " + player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
		foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag ("Player"))
			if (playerObject.GetComponent <PlayerMovement> ().owner == player)
				playerObject.GetComponent <PlayerMovement> ().RPCDestroy ();
	}
}
