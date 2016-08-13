using UnityEngine;
using UnityEngine.Networking;

public enum PlayerColor { R = 0, G, B, Y, }

public class Player : MonoBehaviour
{
    public delegate void OnSpawnEvent(int playerId);
    public static event OnSpawnEvent OnSpawn;
    public delegate void OnDieEvent(int playerId);
    public static event OnDieEvent OnDie;

    public int playerId;
    public NetworkIdentity networkId;
    public GameObject[] modelsByColor;

    void Start()
    {
        if (networkId != null && networkId.isLocalPlayer)
        {
            // && networkId.clientAuthorityOwner != null
            // playerId = networkId.clientAuthorityOwner.connectionId;
            // playerLocal.playerId = playerId;
            var playerLocal = gameObject.AddComponent<PlayerLocal>();
            var inputProcessor = gameObject.AddComponent<PlayerInputProcessor>();
            inputProcessor.playerLocal = playerLocal;
            // TODO: set control scheme
        }

        // TODO: set color from manager
        SetColor(PlayerColor.R);
        if (OnSpawn != null) OnSpawn(playerId);
    }

    void OnDestroy()
    {
        if (OnDie != null) OnDie(playerId);
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
}
