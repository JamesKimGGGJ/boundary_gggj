using UnityEngine;

public class FadeLight : MonoBehaviour {
    Light _light;
    public float time = 0.3f;
    public float intensity;
    public bool disableAfterTime = true;

	// Use this for initialization
	void OnEnable () {
	   _light = GetComponent<Light>();
       _light.intensity = intensity;
	}
	
	// Update is called once per frame
	void Update () {
	   _light.intensity -= intensity * Time.deltaTime / time;
       if(_light.intensity<=0 && disableAfterTime) gameObject.SetActive(false);
	}
}
