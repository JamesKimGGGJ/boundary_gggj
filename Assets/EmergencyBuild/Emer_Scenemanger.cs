using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;
public class Emer_Scenemanger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//restart
		if (Input.GetKeyDown ("r")) {
			Destroy (gameObject);
			SceneManager.LoadScene (0);
		} else if(Input.GetButtonDown("Cancel")){
			Application.Quit();
		}
	}
}
