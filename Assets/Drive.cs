using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Drive : MonoBehaviour {

	public float speed = 10f;
	public float rotationSpeed = 100f;

	// Update is called once per frame
	void Update () {
		float translation = Input.GetAxis("Vertical1") * speed * Time.deltaTime;
		float rotation = Input.GetAxis("Horizontal1") * rotationSpeed * Time.deltaTime;

		transform.Translate(0, 0, translation);
		transform.Rotate(0, rotation, 0);

	}
}
