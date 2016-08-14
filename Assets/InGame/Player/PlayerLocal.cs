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
        if (distance > GameGlobalVar.stormRadius)
        {
            PlayerInputProcessor input = GetComponent<PlayerInputProcessor>();
            if(input!=null) Destroy(input);
            Invoke("DestroyPlayer",1f);
            FindObjectOfType<CameraMove>().targetIsDead = true;
        }
    }

    void DestroyPlayer()
    {
        player.CmdDie();
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
        itemBox.CmdEatAndDestroy();
    }
}
