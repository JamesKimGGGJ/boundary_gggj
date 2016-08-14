using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientGameManager : NetworkBehaviour
{
    public static ClientGameManager inst;

    public readonly Dictionary<int, int> connIdToPlayerOrder = new Dictionary<int, int>();
    public readonly Dictionary<int, int> playerOrderToConnId = new Dictionary<int, int>();

    private const float stormRaduisDescreaseSpeed = 1;
    public bool stormRadiusDecreaseEnabled;
    public StormEffect stormEffect;

    void Awake()
    {
        if (inst == null)
            inst = this;
        else
            Debug.LogWarning("inst not null");
    }

    void Start()
    {
        GameGlobalVar.Reset();
        stormEffect.stormSize = GameGlobalVar.stormRadius;
    }

    void OnDestroy()
    {
        if (inst != this)
            inst = this;
        else
            Debug.LogWarning("inst not match");
    }

    void Update()
    {
        if (stormRadiusDecreaseEnabled)
        {
            GameGlobalVar.stormRadius -= Time.deltaTime * stormRaduisDescreaseSpeed;
            stormEffect.stormSize = GameGlobalVar.stormRadius;
        }
    }

    [ClientRpc]
    public void RpcBindConnIdAndPlayerOrder(int connId, int playerOrder)
    {
        Debug.Log("connId: " + connId + " playerId: " + playerOrder);
        connIdToPlayerOrder[connId] = playerOrder;
        playerOrderToConnId[playerOrder] = connId;
    }

    [ClientRpc]
    public void RpcStartRadiusDecrease()
    {
        stormRadiusDecreaseEnabled = true;
    }
}