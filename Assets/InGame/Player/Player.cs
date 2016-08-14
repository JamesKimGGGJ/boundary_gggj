using UnityEngine;
using UnityEngine.Networking;

public enum PlayerColor { R = 0, G, B, Y, }

public class Player : NetworkBehaviour
{
    public delegate void OnSpawnEvent(int playerId, GameObject player);
    public static event OnSpawnEvent OnSpawn;
	public delegate void OnDieEvent(int playerId, GameObject player);
    public static event OnDieEvent OnDie;

    public NetworkIdentity networkId;
    public Rigidbody2D rb;
    public GameObject[] modelsByColor;
    public PlayerItemShooter itemShooter;

    public CameraMove mainCamera;

    [HideInInspector]
    public int serverPlayerId;

    void Awake()
    {
        networkId = GetComponent<NetworkIdentity>();
        rb = GetComponent<Rigidbody2D>();

    }

    void Start()
    {
        if (networkId != null && networkId.clientAuthorityOwner != null)
            serverPlayerId = networkId.clientAuthorityOwner.connectionId;

        if (networkId != null && networkId.isLocalPlayer)
        {
            var playerLocal = gameObject.AddComponent<PlayerLocal>();
            playerLocal.player = this;
            var inputProcessor = gameObject.AddComponent<PlayerInputProcessor>();
            inputProcessor.player = this;
            mainCamera = Camera.main.GetComponent<CameraMove>();
            mainCamera.target = this.transform;
        }

        if (OnSpawn != null) OnSpawn(serverPlayerId, this.gameObject);
    }

    void OnDestroy()
    {
		if (OnDie != null) OnDie(serverPlayerId, this.gameObject);
    }

    [ClientRpc]
    public void RpcSetColor(PlayerColor color)
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

    [Command]
    public void CmdDie()
    {
        // TODO:
        // effect
        NetworkServer.Destroy(gameObject);
    }

    [Command]
    public void CmdImpulse(Vector2 impulse)
    {
        RpcImpulse(impulse);
    }

    [ClientRpc]
    private void RpcImpulse(Vector2 impulse)
    {
        if (!localPlayerAuthority) return;
        rb.AddForce(impulse, ForceMode2D.Impulse);
    }

    [Command]
    public void CmdGetAndDestroyItem(NetworkIdentity netId)
    {
        if (netId == null) return;
        var itemBox = netId.GetComponent<ItemBox>();
        RpcGetItem(serverPlayerId, itemBox.itemType);
        itemBox.RpcPlayParticle();
        NetworkServer.Destroy(netId.gameObject);
    }

    [ClientRpc]
    private void RpcGetItem(int playerId, ItemType itemType)
    {
        PlayerItemManager.inst.Set(playerId, itemType);
        Debug.Log("item: " + itemType);
    }

    [Command]
    public void CmdRequestFire()
    {
        var itemType = PlayerItemManager.inst.FindAndUnSet(serverPlayerId);
        if (!itemType.HasValue) return;
        itemShooter.ShootServerSide(itemType.Value);
        RpcResponseFire(serverPlayerId, itemType.Value);
    }

    [ClientRpc]
    private void RpcResponseFire(int playerId, ItemType itemType)
    {
        var orgItemType = PlayerItemManager.inst.FindAndUnSet(playerId);
        if (itemType != orgItemType) Debug.LogWarning("item type does not match");
        if (localPlayerAuthority) itemShooter.ShootMySide(itemType);
        itemShooter.ShootClientSide(itemType);
    }

    [Command]
    public void CmdConnId()
    {
        var connId = networkId.clientAuthorityOwner.connectionId;
        RpcConnId(networkId, connId);
    }

    [ClientRpc]
    private void RpcConnId(NetworkIdentity netId, int connId)
    {
        if (networkId == netId)
            ClientGameManager.inst.myConnId = connId;
    }
}
