using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour {
	public HostGameManager hostGameManager;

	// Use this for initialization
	void Start () {
		hostGameManager = GameObject.FindObjectOfType<HostGameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		var distance = ((Vector2)transform.position).magnitude;
		if (distance > 15)
		{
			Destroy(gameObject);
		}
	}

	void OnDestroy(){
		if (hostGameManager) 
		{
			hostGameManager.TryGameOverAfterUpdate();
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		
	}
}
