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
        PlayParticle();
    }

    void Update()
    {
        rendererRoot.Rotate(rotateSpeed * Time.deltaTime);
    }

    public void PlayParticle()
    {
        var getEffect = EffectSpawner.instance.GetEffect("ItemGet");
        getEffect.transform.position = transform.position;
        getEffect.SetActive(true);
    }

    [ClientRpc]
    public void RpcPlayParticle()
    {
        PlayParticle();
    }
}
