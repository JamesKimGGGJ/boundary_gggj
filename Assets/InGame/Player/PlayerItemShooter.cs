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
    public void ShootClientSide(Player player)
    {
        // effect spawn
        throw new NotImplementedException();
    }

    public void ShootMySide(Player player)
    {
        // set velocity
        throw new NotImplementedException();
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
             { ItemType.Rocket, new PlayerRocketShooter(prefabRocket) }
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