using UnityEngine;

public class PlayerLocal : MonoBehaviour
{
    public PlayerId playerId;

    public void Fire()
    {
        var item = PlayerItemManager.inst.Find(playerId);
        // TODO: network
        Debug.Log("fire: " + item);
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
