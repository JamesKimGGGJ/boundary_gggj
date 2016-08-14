using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {
	public Transform target;
	public bool targetIsDead = false;

	private Vector3 centerPoint = Vector3.zero;
	[SerializeField]
	private float distance = 5.0f;

	//0.0f ~ 1.0f
	[SerializeField]
	private float slider = 0.9f;

	private float stormRadius;
	private float fov;
	
	// Update is called once per frame
	void LateUpdate () {
		if (!target || targetIsDead) {
			Vector3 zeroDir = Vector3.zero;
			float zeroVel = 0.0f;
			Vector3 returnToCenter = Vector3.SmoothDamp (transform.position, centerPoint - Vector3.up * 10f, ref zeroDir, 0.3f);
			transform.position = new Vector3 (returnToCenter.x, returnToCenter.y, transform.position.z);
			Camera.main.fieldOfView = Mathf.SmoothDamp (Camera.main.fieldOfView, 60f, ref zeroVel, 0.3f);
			return;
		}

		Vector3 targetFollowVector = new Vector3 (target.position.x, target.position.y - distance, transform.position.z);
		targetFollowVector = Vector3.Slerp (transform.position, targetFollowVector, slider);
		transform.position = targetFollowVector;

		stormRadius = GameGlobalVar.stormRadius;
		fov =  60f - (15f - stormRadius);
		Camera.main.fieldOfView = fov;

	}
}
