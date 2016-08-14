using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LobbyPlayer : NetworkLobbyPlayer {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(transform.gameObject);
        Debug.Log(slot.ToString() + ", isLocalPlayer : " + isLocalPlayer.ToString());
        if (isLocalPlayer)
        {
            SendReadyToBeginMessage();
        }
    }
}
