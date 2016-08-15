using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HostGameManager : NetworkBehaviour
{
    private const float stormRadiusDecreaseStartTime = 20;
    private const float stormRaduisDescreaseSpeed = 1;

    public static HostGameManager instance;
    public ClientGameManager clientGameManager;
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
        StartCoroutine(CoroutineRadiusDecrease());
    }

    private IEnumerator CoroutineRadiusDecrease()
    {
        yield return new WaitForSeconds(stormRadiusDecreaseStartTime);
        while (true)
        {
            var delta = Time.deltaTime * stormRaduisDescreaseSpeed;
            var newRadious = GameGlobalVar.stormRadius - delta;
            clientGameManager.RpcSetStormRadius(newRadious);
            yield return new WaitForSeconds(0.1f);
        }
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
        Debug.Log("AlivePlayers : " + alivePlayers.Count);
    }

    public void OnPlayerDie(int playerId, GameObject player)
    {
        //Die Effect Sound
        audiosrc.Play();

        Debug.Log("Player " + playerId + " Died");
        Debug.Log("AlivePlayers : " + alivePlayers.Count);
        if (!alivePlayers.Remove(playerId))
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
