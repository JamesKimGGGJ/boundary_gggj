using UnityEngine;
using UnityEngine.Networking;

public enum PlayerColor { R = 0, G, B, Y, }

public class Player : NetworkBehaviour
{
    public delegate void OnSpawnEvent(int playerId);
    public static event OnSpawnEvent OnSpawn;
    public delegate void OnDieEvent(int playerId);
    public static event OnDieEvent OnDie;

    public NetworkIdentity networkId;
    public GameObject[] modelsByColor;
    public PlayerItemShooter itemShooter;

    [HideInInspector]
    public int serverPlayerId;

    void Start()
    {
        if (networkId != null && networkId.clientAuthorityOwner != null)
            serverPlayerId = networkId.clientAuthorityOwner.connectionId;

        if (networkId != null && networkId.isLocalPlayer)
        {
            // && networkId.clientAuthorityOwner != null
            // playerId = networkId.clientAuthorityOwner.connectionId;
            // playerLocal.playerId = playerId;
            var playerLocal = gameObject.AddComponent<PlayerLocal>();
            playerLocal.player = this;

            var inputProcessor = gameObject.AddComponent<PlayerInputProcessor>();
            inputProcessor.player = this;
            // TODO: set control scheme
        }

        // TODO: set color from manager
        SetColor(PlayerColor.R);
        if (OnSpawn != null) OnSpawn(serverPlayerId);
    }

    void OnDestroy()
    {
        if (OnDie != null) OnDie(serverPlayerId);
    }

    public void SetColor(PlayerColor color)
    {
        if (modelsByColor.Length <= (int)color)
        {
            Debug.LogError("model for color not found: " + color);
            return;
        }

        foreach (var model in modelsByColor)
            model.SetActive(false);
        modelsByColor[(int)color].SetActive(true);
    }

    public void Die()
    {
        // TODO:
        // effect
        Destroy(gameObject);
    }

    [Command]
    public void CmdGetItem(ItemType itemType)
    {
        RpcGetItem(serverPlayerId, itemType);
    }

    [ClientRpc]
    private void RpcGetItem(int playerId, ItemType itemType)
    {
        PlayerItemManager.inst.RpcSet(playerId, itemType);
        Debug.Log("item: " + itemType);
    }

    [Command]
    public void CmdRequestFire()
    {
        var itemType = PlayerItemManager.inst.Find(serverPlayerId);
        // TODO: debug
        itemType = ItemType.Rocket;
        if (!itemType.HasValue) return;

        itemShooter.ShootServerSide(itemType.Value);
        RpcResponseFire(serverPlayerId, itemType.Value);
    }

    [ClientRpc]
    private void RpcResponseFire(int playerId, ItemType itemType)
    {
        var orgItemType = PlayerItemManager.inst.FindAndUnSet(playerId);
        if (itemType != orgItemType) Debug.LogWarning("item type does not match");
        itemShooter.ShootClientSide(itemType);
    }
}
