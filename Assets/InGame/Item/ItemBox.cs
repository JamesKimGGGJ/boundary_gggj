using UnityEngine;
using UnityEngine.Networking;

public class ItemBox : NetworkBehaviour
{
    [SyncVar]
    public ItemType itemType;
    public Transform rendererRoot;
    public Vector3 rotateSpeed;

    void Update()
    {
        rendererRoot.Rotate(rotateSpeed * Time.deltaTime);
    }
}
