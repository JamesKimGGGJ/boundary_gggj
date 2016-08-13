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

    void OnColliderEnter2D(Collider2D collider)
    // private void OnTriggerEnter2D(Collider2D collider)
    {
        // TODO: network
        if (collider.tag != "Item") return;
        var itemBox = collider.GetComponent<ItemBox>();
        Debug.Log("get item: " + itemBox.itemType);
        PlayerItemManager.inst.Set(playerId, itemBox.itemType);
        // TODO: destroy on network
        Destroy(collider.gameObject);
    }
}
