using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class NetworkController : MonoBehaviour {
	public static string gameType = "Test Project Please Ignore";
	public static uint port = 10768;

	public GameObject serversPanel;
	public Transform contentPanel;
	public GameObject serverButton;

	public string gameName = "Test Server Please Ignore";
	public string password = "867-5309";
	public string comment = "";
	
	public void HostButtonClick () {
		Debug.Log ("Starting Server");
		Network.incomingPassword = password;
		bool useNat = !Network.HavePublicAddress();
		Network.InitializeServer(32, (int)port, useNat);
	}
	public void OnServerInitialized () {
		Debug.Log ("Server Initialized");
		Debug.Log ("Registering Server with Master Server");
		MasterServer.RegisterHost(gameType, gameName, comment);
	}
	
	public void ClientButtonClick () {
		Debug.Log ("Refreshing Hosts");
		MasterServer.ClearHostList ();
		MasterServer.RequestHostList (gameType);
	}
	private void OnHostsRefreshed () {
		HostData[] hosts = MasterServer.PollHostList ();
		PopulateHostList (hosts);
	}
	private void PopulateHostList (HostData[] hosts) {
		serversPanel.SetActive (true);
		foreach (Transform child in contentPanel)
			GameObject.Destroy (child.gameObject);
		Debug.Log (hosts.Length + " games of Game Type " + gameType);
		foreach (HostData host in hosts) {
			Debug.Log (host.gameName + " (passwordProtected: " + host.passwordProtected + ")"
				+ "\n\tplayers: " + host.connectedPlayers + " / " + host.playerLimit
				+ "\n\tip: " + host.ip + ":" + host.port
				+ "\n\tuseNAT: " + host.useNat + " GUID: " + host.guid
				+ "\n\tdescription: " + host.comment);
			GameObject newButton = Instantiate(serverButton) as GameObject;
			ServerButtonController serverButtonController = newButton.GetComponent <ServerButtonController> ();
			serverButtonController.host = host;
			serverButtonController.gameLabel.text = host.gameName;
			serverButtonController.playersLabel.text = host.connectedPlayers + " / " + host.playerLimit;
			serverButtonController.commentLabel.text = host.comment;
			serverButtonController.keyIcon.SetActive (host.passwordProtected);
			serverButtonController.networkController = this;
			newButton.transform.SetParent (contentPanel);
		}
	}

	public void OnMasterServerEvent (MasterServerEvent msEvent) {
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
	public void OnFailedToConnectToMasterServer (NetworkConnectionError info) {
		Debug.Log ("Master Server Connection Failed: " + info);
	}
}
