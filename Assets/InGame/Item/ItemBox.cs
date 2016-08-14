using UnityEngine;
using UnityEngine.Networking;

public class ItemBox : NetworkBehaviour
{
    [SyncVar]
    public ItemType itemType;
    public Transform rendererRoot;
    public Vector3 rotateSpeed;

    void Start()
    {
        var getEffect = EffectSpawner.instance.GetEffect("ItemGet");
        getEffect.transform.position = transform.position;
        getEffect.SetActive(true);
    }

    void Update()
    {
        rendererRoot.Rotate(rotateSpeed * Time.deltaTime);
    }

    [Command]
    public void CmdEatAndDestroy()
    {
        var getEffect = EffectSpawner.instance.GetEffect("ItemGet");
        getEffect.transform.position = transform.position;
        getEffect.SetActive(true);
        NetworkServer.Destroy(gameObject);
    }
}
