using UnityEngine;
using System.Collections.Generic;

public class StormEffect : MonoBehaviour
{
    public GameObject stormElement;
    public float stormSize;
    public float stormElementWidth = 5;
    private List<GameObject> stormList;
    private float prevStormSize;
    void Awake()
    {
		stormList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (prevStormSize == stormSize) return;
        int stormElementCount = Mathf.RoundToInt(stormSize * Mathf.PI * 2 / stormElementWidth);
		if (stormList.Count != stormElementCount)
		{
			SetStormListCount(stormElementCount);
		}
		ResetStorms();
		prevStormSize = stormSize;
    }

	void SetStormListCount(int Count)
	{
		for(int i = stormList.Count; i<Count; i++)
		{
			GameObject newObj = Instantiate<GameObject>(stormElement);
			newObj.transform.SetParent(transform);
			ParticleSystem particle = newObj.GetComponentInChildren<ParticleSystem>();
			particle.randomSeed = (uint)(Random.value * uint.MaxValue);
			particle.Simulate(0,true,true);
			particle.Play();
			stormList.Add(newObj);
		}
		for(int i = stormList.Count; i>Count && i>0; i--)
		{
			Destroy(stormList[0]);
			stormList.RemoveAt(0);
		}
	}

	void ResetStorms()
	{
		float unitDeg = 2 * Mathf.PI / stormList.Count;
		for(int i=0; i<stormList.Count; i++)
		{
			float deg = unitDeg * i * Mathf.Rad2Deg;
			Quaternion quat = Quaternion.Euler(0,0,deg);
			Vector2 direction = (Vector2)(quat * Vector2.up);
			stormList[i].transform.position = direction * stormSize;
			stormList[i].transform.rotation = Quaternion.Euler(0,0,deg+90);
		}
	}
}
