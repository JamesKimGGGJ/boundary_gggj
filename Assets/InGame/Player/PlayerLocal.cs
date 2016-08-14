using UnityEngine;
using UnityEngine.Networking;

public class PlayerLocal : MonoBehaviour
{
    public Player player;

    void Update()
    {
        TryDestroyByStorm();
    }

    void TryDestroyByStorm()
    {
        var distance = ((Vector2)transform.position).magnitude;
        if (distance > GameGlobalVar.stormRadius) player.CmdDie();
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
        NetworkServer.Destroy(itemBox.gameObject);
    }
}
