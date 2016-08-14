using UnityEngine;
using UnityEngine.Networking;

public class Rocket : NetworkBehaviour
{
    public Rigidbody2D rb;
    public float lifeTime = 7;
    public GameObject prefabExplode;

    void Update()
    {
        UpdateLifeTime();
        UpdateAngle();
    }

    void UpdateLifeTime()
    {
        if (!localPlayerAuthority) return;
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0) DestroyOnNetwork();
    }

    void UpdateAngle()
    {
        var dir = Mathf.Atan2(rb.velocity.y, rb.velocity.x);
        transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * dir);
    }

    void DestroyOnNetwork()
    {
        NetworkServer.UnSpawn(gameObject);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!localPlayerAuthority) return;
        if (col.tag != "Player") return;
        Explode();
    }

    void Explode()
    {
        var players = FindObjectsOfType<Player>();
        foreach (var player in players)
        {
            var delta = player.transform.position - transform.position;
            var dist = delta.magnitude;
            if (dist > 3) continue;
            var impulse = (3 - dist) * delta.normalized * 8;
            player.rb.AddForce(impulse, ForceMode2D.Impulse);
        }

        CmdExplode();
    }

    [Command]
    private void CmdExplode()
    {
        RpcExplode();
    }

    [ClientRpc]
    private void RpcExplode()
    {
        var explodeFX = Instantiate(prefabExplode, transform.position, Quaternion.identity);
        Destroy(explodeFX, 3);
        DestroyOnNetwork();
    }
}
