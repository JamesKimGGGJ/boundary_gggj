using UnityEngine;
using UnityEngine.Networking;

public enum PlayerColor { R = 0, G, B, Y, }

public class Player : NetworkBehaviour
{
    public delegate void OnSpawnEvent(int playerId, GameObject player);
    public static event OnSpawnEvent OnSpawn;
    public delegate void OnDieEvent(int playerId);
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
        rb = GetComponent<Rigidbody2D>();

    }

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

			mainCamera = Camera.main.GetComponent<CameraMove> ();
			mainCamera.target = this.transform;
            // TODO: set control scheme
        }

        // TODO: set color from manager
        RpcSetColor(PlayerColor.R);
        if (OnSpawn != null) OnSpawn(serverPlayerId, this.gameObject);
    }

    void OnDestroy()
    {
        if (OnDie != null) OnDie(serverPlayerId);
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
    public void CmdGetItem(ItemType itemType)
    {
        RpcGetItem(serverPlayerId, itemType);
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
    public void CmdDestroy(NetworkIdentity networkId)
    {
        NetworkServer.Destroy(networkId.gameObject);
    }
		
}
