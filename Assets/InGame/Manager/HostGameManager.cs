using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HostGameManager : NetworkBehaviour
{
    public ClientGameManager clientGameManager;
    private const float stormRadiusDecreaseStartTime = 20;
    private readonly List<int> alivePlayers = new List<int>();

    void Start()
    {
        Invoke("StartRaduisDecrease", stormRadiusDecreaseStartTime);
    }

    private void StartRaduisDecrease()
    {
        Debug.Log("StartRaduisDecrease");
        clientGameManager.RpcStartRadiusDecrease();
    }

    public void OnPlayerSpawn(int playerId)
    {
        if (alivePlayers.Contains(playerId))
        {
            Debug.LogWarning("add same player guid: " + playerId);
            return;
        }

        alivePlayers.Add(playerId);
    }

    public void OnPlayerDie(int playerId)
    {
        Debug.Log("Player " + playerId + " Died");
        alivePlayers.Remove(playerId);
        if (alivePlayers.Count == 1)
        {
            Win(alivePlayers[0]);
        }
        else if (alivePlayers.Count == 0)
        {
            Debug.LogWarning("No Player Exist");
        }
    }

    private void Win(int winnerId)
    {
        GameMessagePasser.inst.RpcWin(winnerId);
    }
}
