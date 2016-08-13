using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class HostGameManager : NetworkBehaviour {

	private List<int> netIdList = new List<int>();
	void Awake(){
		ClientPlayer.OnPlayerSpawn += OnPlayerSpawn;
		ClientPlayer.OnPlayerDie += OnPlayerDie;
	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(isServer){
			this.gameObject.SetActive(true);
		}
		else{
			this.gameObject.SetActive(false);
		}
				
		Debug.Log(netIdList.Count + " of players exist");

	}

	void OnPlayerSpawn(int netId){
		if (!isServer)
			return;
		Debug.Log("Player " + netId + " Spawned");
		netIdList.Add(netId);
	}

	void OnPlayerDie(int netId){
		if (!isServer)
			return;
		Debug.Log("Player " + netId + " Died");
		netIdList.Remove(netId);
		if (netIdList.Count == 1) {
			int winnerId = -1;
			for (int i = 0; i < netIdList.Count; i++) {
				if (netIdList[i] != -1) {
					winnerId = netIdList [i];
				}
			}
			Debug.Log ("Player " + winnerId + " Won");

		} else if (netIdList.Count == 0) {
			Debug.Log ("No Player Exist");
		}
			
	}

/*
	bool CheckGameOver(out int winner)
	{
		winner = -1;

		GameObject[] playerGameObject = GameObject.FindGameObjectsWithTag("Player");
		int count = playerGameObject.Length;
		if (count == 0) {
			Debug.LogError("no player");
			return false;
		}

		if (count != 1)
			return false;

		var netId = playerGameObject[0].GetComponent<NetworkIdentity>();
		if (netId == null)
		{
			// TODO: 나거나 혹은 네트워크를 통해서 생성된게 아니다.
			winner = -1;
			return true;
		}

		winner = netId.clientAuthorityOwner.connectionId;
		return true;
	}

	//게임오버
	private  void TryGameOver(){
		int winner;
		if (CheckGameOver(out winner))
		{
			Debug.Log("game over: winner " + winner);
		}
	}


	public void TryGameOverAfterUpdate()
	{
		Invoke("TryGameOver", 0.1f);
	}
	*/
}
