using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HostGameManager : NetworkBehaviour
{
    public ClientGameManager clientGameManager;
    private const float stormRadiusDecreaseStartTime = 20;
    private readonly List<int> alivePlayers = new List<int>();
    private readonly List<GameObject> players = new List<GameObject>();
    private AudioSource audiosrc;

    void Awake()
    {
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
        SetColor();

        clientGameManager.RpcBindConnIdAndPlayerOrder(playerId, players.Count);
    }

	public void OnPlayerDie(int playerId, GameObject player)
    {
        //Die Effect Sound
        audiosrc.Play();

        Debug.Log("Player " + playerId + " Died");
        alivePlayers.Remove(playerId);
		players.Remove (player);
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

    public void SetColor()
    {
        for (int i = 0; i < players.Count; i++)
        {
            var player = players[i].GetComponent<Player>();
            switch (i)
            {
                case 0: player.RpcSetColor(PlayerColor.R); break;
                case 1: player.RpcSetColor(PlayerColor.B); break;
                case 2: player.RpcSetColor(PlayerColor.G); break;
                case 3: player.RpcSetColor(PlayerColor.Y); break;
            }
        }
    }
}
