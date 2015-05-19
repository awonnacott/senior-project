using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 [System.Serializable]
public class ServerButtonController : MonoBehaviour {
	public Button serverButton;
	public HostData host;
	public Text gameLabel;
	public Text playersLabel;
	public Text commentLabel;
	public GameObject keyIcon;
	public NetworkController networkController;
}
