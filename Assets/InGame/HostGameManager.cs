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
		if(isServer){
			this.gameObject.SetActive(true);

		}
		else{
			this.gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(netIdList.Count);
	}

	void OnPlayerSpawn(int netId){
		Debug.Log(netId + " Spawned");
		netIdList.Add(netId);
	}

	void OnPlayerDie(int netId){
		Debug.Log(netId + " Died");
		netIdList.Remove(netId);
		if (netIdList.Count == 1) {
			int winnerId = netIdList.IndexOf (0);
			Debug.Log (winnerId + " Won");

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
