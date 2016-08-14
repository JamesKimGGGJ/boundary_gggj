using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections;
using System.Collections.Generic;

public class Lobby : NetworkLobbyManager{

    private int hello;

    public void CreateMatch() {
        StartMatchMaker();
        matchMaker.CreateMatch("Room", 4,true, "", "", "", 0, 1, OnMatchCreate);
    }
    //call this method to find a match through the matchmaker
    public void FindInternetMatch( )
    {
        StartMatchMaker();
        matchMaker.ListMatches(0, 20, "Room", false, 0, 1, OnMatchList);
    }

    override public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList) {
        if (success) {
            if (matchList.Count != 0)
            {
                matchMaker.JoinMatch(matchList[matchList.Count - 1].networkId, "", "", "", 0, 1, OnMatchJoined);
            }
            else {
                Debug.Log("Nothing");
            }
        }
    }

    override public void OnMatchJoined(bool success, string extendedInfo, UnityEngine.Networking.Match.MatchInfo matchInfo) {
        base.OnMatchJoined(success, extendedInfo, matchInfo);
        if (success)
        {
            Debug.Log("Joined Success");
            //Utility.SetAccessTokenForNetwork(matchInfo.networkId, new NetworkAccessToken(matchInfo.accessToken.GetByteString()));
            //client = new NetworkClient();
            //client.RegisterHandler(MsgType.Connect, OnConnected);
            //client.Connect(matchInfo);

        }
            
    }

    public void OnConnected(NetworkMessage msg) {
        Debug.Log("Connected");
    }

    override public void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo) {
        base.OnMatchCreate(success, extendedInfo, matchInfo);
        Debug.Log("Create");
        //Utility.SetAccessTokenForNetwork(matchInfo.networkId, new NetworkAccessToken(matchInfo.accessToken.GetByteString()));
        //NetworkServer.Listen(matchInfo, 7777);
    }

    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject obj = base.OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
        return obj;
    }

    public override void OnLobbyServerPlayersReady()
    {
        if (maxPlayers <= lobbySlots.Length)
        {
            bool allready = true;
            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                if (lobbySlots[i] != null)
                    allready &= lobbySlots[i].readyToBegin;
            }

            if (allready)
               ServerChangeScene(playScene);
        }
    }

    void Start()
    {
        hello = 0;
    }
    void Update()
    {
        if(hello > 100)
        {
            Debug.Log(numPlayers);
            /*if (maxPlayers <= numPlayers)
            {
                //bool allready = true;

                for (int i = 0; i < maxPlayers; i++) {
                    lobbySlots[i].readyToBegin = true;
                }
                ServerChangeScene(playScene);
            }*/
            hello -= 100;
        }
        else
        {
            hello++;
        }
    }
}
