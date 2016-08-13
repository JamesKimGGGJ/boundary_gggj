using UnityEngine;

public class PlayerLocal : MonoBehaviour
{
    private const float stormDistance = 15;
    public int playerId;

    void Update()
    {
        TryDestroyByStorm();
    }

    public void Fire()
    {
        var item = PlayerItemManager.inst.FindAndUnSet(playerId);
        // TODO: network
        Debug.Log("fire: " + item);
    }

    void TryDestroyByStorm()
    {
        var distance = ((Vector2)transform.position).magnitude;
        if (distance > stormDistance) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // TODO: network
        if (collider.tag == "Item")
            OnCollideWithItem(collider.GetComponent<ItemBox>());
    }

    private void OnCollideWithItem(ItemBox itemBox)
    {
        Debug.Log("get item: " + itemBox.itemType);
        PlayerItemManager.inst.Set(playerId, itemBox.itemType);
        // TODO: destroy on network
        Destroy(itemBox.gameObject);
    }
}
