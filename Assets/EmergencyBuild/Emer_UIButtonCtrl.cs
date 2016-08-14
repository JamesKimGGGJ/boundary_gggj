using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Emer_UIButtonCtrl : MonoBehaviour {

	public Transform cameraSecondPosition;
	// Use this for initialization
	void Start () {

	}

	public void OnStartButton(){
		DOTween.Init();
		Camera.main.transform.DOMove(cameraSecondPosition.position,1).SetEase(Ease.OutCubic);
		Camera.main.transform.DORotate(cameraSecondPosition.rotation.eulerAngles,1).OnComplete(MoveToNextScene);
	}

	void MoveToNextScene(){
		SceneManager.LoadScene (1);
	}
}
