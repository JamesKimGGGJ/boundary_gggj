using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Messanger : NetworkBehaviour
{
    Lobby lobby;
    UIButtonControl bc;
    public int num;
    
    // Use this for initialization
	void Start () {
        lobby = GameObject.Find("UIObject").GetComponent<Lobby>();
        bc = GameObject.Find("button").GetComponent<UIButtonControl>();
        num = 0;
    }

    [ClientRpc]
    public void RpcMeg(int isNum, int Num)
    {
        if (isNum != 0)
            num = Num;
        else {
            StartCoroutine(bc.startGame());
        }
    }
}
