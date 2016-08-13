using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ClientPlayer : NetworkBehaviour {
	public delegate void OnPlayerDieEvent(int netId);
	public static event OnPlayerDieEvent OnPlayerDie;

	public delegate void OnPlayerSpawnEvent (int netId);
	public static event OnPlayerSpawnEvent OnPlayerSpawn;

	private int clientNetId;

	void Start () {
		clientNetId = this.GetComponent<NetworkIdentity>().clientAuthorityOwner.connectionId;
		OnPlayerSpawn(clientNetId);
	}

	void Update () {
		var distance = ((Vector2)transform.position).magnitude;
		if (distance > 15){			
			Destroy(gameObject);
		}	
	}

	void OnDestroy(){
			OnPlayerDie(clientNetId);		
	}
}
