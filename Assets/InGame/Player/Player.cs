using UnityEngine;
using UnityEngine.Networking;

public enum PlayerColor { R = 0, G, B, Y, }

public class Player : NetworkBehaviour
{
    public delegate void OnSpawnEvent(int playerId);
    public static event OnSpawnEvent OnSpawn;
    public delegate void OnDieEvent(int playerId);
    public static event OnDieEvent OnDie;
    public delegate void OnFireEvent(int playerId);
    public static event OnFireEvent OnFire;

    public NetworkIdentity networkId;
    public GameObject[] modelsByColor;

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
            gameObject.AddComponent<PlayerLocal>();

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
    public void CmdRequestFire()
    {
        RpcResponseFire();
    }

    [ClientRpc]
    private void RpcResponseFire()
    {
        // TODO: fire
        Debug.Log("fire");
    }
}
