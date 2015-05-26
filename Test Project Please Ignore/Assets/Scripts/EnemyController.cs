using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	public GameObject[] players = new GameObject[0];
	public Transform target;
	new private NetworkView networkView;
	bool updatePlayerNow = false;
	//PlayerHealth playerHealth;
	//EnemyHealth enemyHealth;
	NavMeshAgent nav;
	
	public float validationPeriod = 5;

	void Start () {
		networkView = GetComponent <NetworkView> ();
		nav = GetComponent <NavMeshAgent> ();
		if (Network.isServer)
			StartCoroutine (ValidatePosition ());
	}
		
	IEnumerator ValidatePosition () {
		while (true) {
			networkView.RPC ("ClientReceivePosition", RPCMode.Others, transform.position);
			yield return new WaitForSeconds (validationPeriod);
		}
	}

	void OnConnectedToServer () {
		updatePlayerNow = true;
	}
	void OnServerInitialized () {
		updatePlayerNow = true;
	}
	void OnPlayerConnected (NetworkPlayer newPlayer) {
		networkView.RPC ("ClientReceivePosition", newPlayer, transform.position);
		networkView.RPC ("ClientReceiveTarget", newPlayer, nav.destination);
		updatePlayerNow = true;
	}
	void OnPlayerDisconnected () {
		updatePlayerNow = true;
	}

	void OnDisconnectedFromServer () {
		players = new GameObject[0];
		nav.Stop ();
		nav.ResetPath ();
		target = null;
	}

	void Update () {
		if (Network.isServer) {
			if (updatePlayerNow)
				players = GameObject.FindGameObjectsWithTag ("Player");
			foreach (GameObject player in players)
				if ((target == null || nav.remainingDistance > Vector3.Distance (transform.position, player.transform.position)) && !(Physics.Raycast (transform.position, player.transform.position - transform.position, (player.transform.position - transform.position).magnitude)))
					target = player.transform;
			if (target != null && !(Physics.Raycast (transform.position, target.position - transform.position, (target.position - transform.position).magnitude))) {
				nav.SetDestination (target.position);
				networkView.RPC ("ClientReceiveTarget", RPCMode.Others, target.position);
			}
			updatePlayerNow = false;
		}
	}

	[RPC]
	void ClientReceiveTarget (Vector3 targetPosition) {
		nav.SetDestination (targetPosition);
	}
	[RPC]
	void ClientReceivePosition (Vector3 newPosition) {
		transform.position = newPosition;
	}
}
