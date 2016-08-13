using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections;
using System.Collections.Generic;

public class Lobby : NetworkLobbyManager{

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
        if (success)
            Debug.Log("Joined Success");
    }

    override public void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo) {
        Debug.Log("Create");
    }

}
