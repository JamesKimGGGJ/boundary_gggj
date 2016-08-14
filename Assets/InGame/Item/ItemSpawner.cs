using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ItemSpawner : NetworkBehaviour
{
    public ItemBox PrefabItemBox;
    public float SpawnPeriod = 5;

    void OnEnable()
    {
        StartCoroutine(CoroutineSpawn());
    }

    private IEnumerator CoroutineSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(SpawnPeriod);
            if (isServer) Spawn();
        }
    }

    private static Vector3 SamplePosition()
    {
        var r = Random.Range(0, GameGlobalVar.stormRadius);
        var angle = Random.Range(0, 2 * Mathf.PI);
        return new Vector3(r * Mathf.Cos(angle), r * Mathf.Sin(angle), -0.8f);
    }

    private void Spawn()
    {
        var go = (GameObject)Instantiate(PrefabItemBox.gameObject, SamplePosition(), Quaternion.identity);
        var itemBox = go.GetComponent<ItemBox>();
        itemBox.itemType = ItemHelper.Random();
        NetworkServer.Spawn(go);
    }
}