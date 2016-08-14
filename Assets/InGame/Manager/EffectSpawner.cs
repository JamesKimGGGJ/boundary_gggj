using UnityEngine;
using System.Collections.Generic;

public class EffectSpawner : MonoBehaviour
{
    private static EffectSpawner _instance;
    public static EffectSpawner instance
    {
        get
        {
            if(_instance != null)
                return _instance;
            GameObject spawnerObject = new GameObject("EffectSpawner");
            _instance = spawnerObject.AddComponent<EffectSpawner>();
            return _instance;
        }
    }
    Dictionary<string, List<GameObject>> effectPool = new Dictionary<string, List<GameObject>>();

    // Update is called once per frame
    public GameObject GetEffect(string effectName)
    {
        if (!effectPool.ContainsKey(effectName))
        {
            List<GameObject> newList = new List<GameObject>();
            effectPool.Add(effectName, newList);
        }
        GameObject idleEffect;
        effectPool[effectName].RemoveAll(effect => effect==null);
        if (effectPool[effectName].Exists(effect => !effect.activeSelf))
        {
            idleEffect = effectPool[effectName].Find(effect => !effect.activeSelf);
            idleEffect.transform.parent = transform;
        }
        else
        {
            GameObject prefab = Resources.Load<GameObject>(effectName);
            idleEffect = Instantiate(prefab) as GameObject;
            idleEffect.transform.parent = transform;
            effectPool[effectName].Add(idleEffect);
        }
        return idleEffect;
    }
}
