using UnityEngine;
using UnityEngine.Networking;

public class ClientGameManager : NetworkBehaviour
{
    private const float stormRaduisDescreaseSpeed = 1;
    public bool stormRadiusDecreaseEnabled;
    public StormEffect stormEffect;

    void Start()
    {
        stormEffect.stormSize = GameGlobalVar.stormRadius;
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
    public void RpcStartRadiusDecrease()
    {
        stormRadiusDecreaseEnabled = true;
    }
		
}