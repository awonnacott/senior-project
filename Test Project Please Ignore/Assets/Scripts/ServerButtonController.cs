using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 [System.Serializable]
public class ServerButtonController : MonoBehaviour {
	public Button serverButton;
	public Text gameLabel;
	public Text playersLabel;
	public Text commentLabel;
	public GameObject keyIcon;

	NetworkController networkController;
	HostData host;

	public void AssignHost (NetworkController networkController, HostData host) {
		gameLabel.text = host.gameName;
		playersLabel.text = host.connectedPlayers + " / " + host.playerLimit;
		commentLabel.text = host.comment;
		keyIcon.SetActive (host.passwordProtected);

		this.host = host;
		this.networkController = networkController;
		serverButton.onClick.AddListener(ConnectToHost);
	}

	public void ConnectToHost () {
		networkController.ConnectToHost (host);
	}
}
