using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public ItemType itemType;
    public Transform rendererRoot;
    public Vector3 rotateSpeed;

    void Update()
    {
        rendererRoot.Rotate(rotateSpeed * Time.deltaTime);
    }
}
