using UnityEngine;

public class PlayerLocal : MonoBehaviour
{
    private const float stormDistance = 15;
    public Player player;

    void Update()
    {
        TryDestroyByStorm();
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
        player.CmdGetItem(itemBox.itemType);
        Destroy(itemBox.gameObject);
    }
}
