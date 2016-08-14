using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LobbyPlayer : NetworkLobbyPlayer {

	// Use this for initialization
	void Start () {
        if (isLocalPlayer)
            SendReadyToBeginMessage();
    }
}
