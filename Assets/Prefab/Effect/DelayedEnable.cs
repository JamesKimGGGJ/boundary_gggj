using UnityEngine;
using System.Collections;

public class DelayedEnable : MonoBehaviour {
	public GameObject ObjectToEnable;

	// Use this for initialization
	void Start () {
		Invoke("EnableObject",0.3f);
	}
	
	void EnableObject () {
		ObjectToEnable.SetActive(true);
	}
}
