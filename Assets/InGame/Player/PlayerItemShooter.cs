using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public interface IPlayerItemShooter
{
    void ShootServerSide(Player player);
    void ShootClientSide(Player player);
    void ShootMySide(Player player);
}

public class PlayerRocketShooter : IPlayerItemShooter
{
    private const float speed = 5;
    private readonly GameObject prefab;

    public PlayerRocketShooter(GameObject prefab)
    {
        this.prefab = prefab;
    }

    public void ShootServerSide(Player player)
    {
        var pos = player.transform.TransformPoint(Vector2.right);
        pos.z = -0.6f;
        var rot = player.transform.rotation;
        var go = (GameObject)GameObject.Instantiate(prefab, pos, rot);
        var worldVel = go.transform.right * speed;
        var playerVel = player.GetComponent<Rigidbody2D>().velocity;
        go.GetComponent<Rigidbody2D>().velocity = worldVel + (Vector3)playerVel;
        NetworkServer.Spawn(go);
    }

    public void ShootClientSide(Player player)
    {
        // do nothing
    }

    public void ShootMySide(Player player){}
}

public class PlayerJetPackShooter : IPlayerItemShooter
{
    private const float speed = 8;
    public void ShootClientSide(Player player)
    {
        // effect spawn
        GameObject effect = EffectSpawner.instance.GetEffect("JetPack");
        effect.transform.position = player.transform.position;
        effect.transform.rotation = player.transform.rotation;
        effect.SetActive(true);
    }

    public void ShootMySide(Player player)
    {
        // set velocity
        var input = player.GetComponent<PlayerInputProcessor>();
        if(input==null) throw new Exception("Player without Input used Item");
        int x,y;
        input.GetMoveInput(out x, out y);
        player.rb.velocity = new Vector2(x,y).normalized * speed;
    }

    public void ShootServerSide(Player player)
    {
        // do nothing
    }
}

public class PlayerColumnShooter : IPlayerItemShooter
{
    public void ShootClientSide(Player player)
    {
        // effect spawn
        GameObject effect = EffectSpawner.instance.GetEffect("SpawnedColumn");
        effect.transform.position = player.transform.position;
        effect.transform.rotation = Quaternion.Euler(0,0,UnityEngine.Random.Range(0f,360f));
        effect.SetActive(true);
    }

    public void ShootMySide(Player player)
    {
        // do nothing
    }

    public void ShootServerSide(Player player)
    {
        // do nothing
    }
}


public class PlayerItemShooter : MonoBehaviour
{
    public Player player;
    public GameObject prefabRocket;
    private Dictionary<ItemType, IPlayerItemShooter> shooters;

    void Awake()
    {
        shooters = new Dictionary<ItemType, IPlayerItemShooter>{
             { ItemType.Rocket, new PlayerRocketShooter(prefabRocket) },
             { ItemType.JetPack, new PlayerJetPackShooter() },
             { ItemType.ColumnDrop, new PlayerColumnShooter() }
        };
    }

    public void ShootServerSide(ItemType itemType)
    {
        if (!shooters.ContainsKey(itemType))
        {
            Debug.LogWarning("shooter not found: " + itemType);
            return;
        }

        // TODO: fire
        Debug.Log("fire server side: " + itemType);
        shooters[itemType].ShootServerSide(player);
    }

    public void ShootClientSide(ItemType itemType)
    {
        if (!shooters.ContainsKey(itemType))
        {
            Debug.LogWarning("shooter not found: " + itemType);
            return;
        }

        // TODO: fire
        Debug.Log("fire client side: " + itemType);
        shooters[itemType].ShootClientSide(player);
    }

    public void ShootMySide(ItemType itemType)
    {
        if (!shooters.ContainsKey(itemType))
        {
            Debug.LogWarning("shooter not found: " + itemType);
            return;
        }

        // TODO: fire
        Debug.Log("fire My side: " + itemType);
        shooters[itemType].ShootMySide(player);
    }
}