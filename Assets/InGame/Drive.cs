using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Drive : MonoBehaviour {

	public float speed = 10f;
	public float rotationSpeed = 100f;
	private Rigidbody2D rb;

	void Start(){
		rb = GetComponent<Rigidbody2D>();
	}
	// Update is called once per frame
	void Update () {
		float translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
		float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

		rb.AddForce(new Vector3(translation, 0, 0));
		//transform.Translate(translation, 0, 0);
		transform.Rotate(0, 0, rotation);

	}
}
