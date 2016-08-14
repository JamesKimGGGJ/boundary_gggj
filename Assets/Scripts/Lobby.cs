using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections;
using System.Collections.Generic;

public class Lobby : NetworkLobbyManager{
    private static Lobby _instance;
    public static Lobby instance
    {
        get
        {
            if(_instance == null)
                _instance = FindObjectOfType<Lobby>();
            return _instance;
        }
    }
    public void CreateMatch() {
        StartMatchMaker();
        matchMaker.CreateMatch("Room384", 4,true, "", "", "", 0, 1, OnMatchCreate);
    }
    //call this method to find a match through the matchmaker
    public void FindInternetMatch( )
    {
        StartMatchMaker();
        matchMaker.ListMatches(0, 20, "Room384", false, 0, 1, OnMatchList);
    }

    override public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList) {
        if (success) {
            if (matchList.Count != 0)
            {
                matchMaker.JoinMatch(matchList[matchList.Count - 1].networkId, "", "", "", 0, 1, OnMatchJoined);
            }
            else {
                matchMaker.CreateMatch("Room384", 4, true, "", "", "", 0, 1, OnMatchCreate);
                //Debug.Log("Nothing");
            }
        }
    }

    override public void OnMatchJoined(bool success, string extendedInfo, UnityEngine.Networking.Match.MatchInfo matchInfo) {
        base.OnMatchJoined(success, extendedInfo, matchInfo);
        if (success)
        {
            GameObject.Find("button").GetComponent<UIButtonControl>().Ready = true;
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
        GameObject.Find("button").GetComponent<UIButtonControl>().Ready = true;
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
            GameObject.Find("Messanger").GetComponent<Messanger>().RpcMeg(1, numPlayers);
            GameObject.Find("button").GetComponent<UIButtonControl>().AllReady = allready;
        }
    }
    private static int _playerId = int.MinValue;
    public static int playerId
    {
        get
        {
            if(_playerId==int.MinValue)
                _playerId = GetPlayerId();
            return _playerId;
        }
    }
    private static int GetPlayerId()
    {
        LobbyPlayer[] players = FindObjectsOfType<LobbyPlayer>();
        foreach(var player in players)
        {
            if(player.isLocalPlayer)
                return player.slot;
        }
        throw new System.Exception("No Local LobbyPlayer Found");
    }
}
