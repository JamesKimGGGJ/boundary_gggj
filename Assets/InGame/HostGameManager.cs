using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class HostGameManager : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

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
}
