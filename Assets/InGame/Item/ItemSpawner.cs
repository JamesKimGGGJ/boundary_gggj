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
        // TODO: radius
        return new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), -0.8f);
    }

    private void Spawn()
    {
        var go = (GameObject)Instantiate(PrefabItemBox.gameObject, SamplePosition(), Quaternion.identity);
        var itemBox = go.GetComponent<ItemBox>();
        itemBox.itemType = ItemHelper.Random();
        NetworkServer.Spawn(go);
    }
}