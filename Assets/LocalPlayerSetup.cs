using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LocalPlayerSetup : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		if(isLocalPlayer){
			GetComponent<Drive>().enabled = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
