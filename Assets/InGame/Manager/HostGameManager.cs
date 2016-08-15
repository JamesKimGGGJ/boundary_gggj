using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HostGameManager : NetworkBehaviour
{
    public static HostGameManager instance;
    public ClientGameManager clientGameManager;
    private const float stormRadiusDecreaseStartTime = 20;
    private readonly List<int> alivePlayers = new List<int>();
    private readonly List<GameObject> players = new List<GameObject>();
    private AudioSource audiosrc;

    void Awake()
    {
        instance = this;
        audiosrc = GetComponent<AudioSource>();
    }
    void Start()
    {
        Invoke("StartRaduisDecrease", stormRadiusDecreaseStartTime);
    }

    private void StartRaduisDecrease()
    {
        Debug.Log("StartRaduisDecrease");
        clientGameManager.RpcStartRadiusDecrease();
    }

    public void OnPlayerSpawn(int playerId, GameObject player)
    {
        if (alivePlayers.Contains(playerId))
        {
            Debug.LogWarning("add same player guid: " + playerId);
            return;
        }

        alivePlayers.Add(playerId);
        players.Add(player);
        Debug.Log("AlivePlayers : "+alivePlayers.Count);
    }

    public void OnPlayerDie(int playerId, GameObject player)
    {
        //Die Effect Sound
        audiosrc.Play();

        Debug.Log("Player " + playerId + " Died");
        Debug.Log("AlivePlayers : "+alivePlayers.Count);
        if(!alivePlayers.Remove(playerId))
            Debug.LogWarning("No player died but someone died");
        
        players.Remove(player);
        if (alivePlayers.Count == 1)
        {
            Win(alivePlayers[0]);
        }
        else if (alivePlayers.Count <= 0)
        {
            Debug.LogWarning("No Player Exist");
            Win(-1);
        }
    }

    private void Win(int winnerId)
    {
        clientGameManager.RpcWin(winnerId);
    }
}
