using UnityEngine;
using UnityEngine.Networking;

public enum PlayerColor { R = 0, G, B, Y, }

public class Player : MonoBehaviour
{
    public NetworkIdentity networkId;
    public PlayerId playerId;
    public GameObject[] modelsByColor;

    void Start()
    {
        // TODO: set player id from manager
        playerId = (PlayerId)1;

        if (networkId != null && networkId.isLocalPlayer)
        {
            var playerLocal = gameObject.AddComponent<PlayerLocal>();
            playerLocal.playerId = playerId;
            var inputProcessor = gameObject.AddComponent<PlayerInputProcessor>();
            inputProcessor.playerLocal = playerLocal;
            // TODO: set control scheme
        }

        // TODO: set color from manager
        SetColor(PlayerColor.R);
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
}
