using UnityEngine;

public class GameOver : MonoBehaviour {
	public static GameOver instance;
	public UnityEngine.UI.Text textbox;
	public string winMessage;
	public string loseMessage;

	// Use this for initialization
	void Awake () {
		instance = this;
		gameObject.SetActive(false);
	}
	
	public void SetGameOver (bool win) {
		gameObject.SetActive(true);
		if(win)
			textbox.text = winMessage;
		else
			textbox.text = loseMessage;
		Time.timeScale = 0.2f;
	}

	public void RestartGame()
	{
		Time.timeScale = 1f;
		var manager = FindObjectOfType<UnityEngine.Networking.NetworkManager>();
		if(manager!=null)
		{
			Destroy(manager.gameObject);
			UnityEngine.Networking.NetworkManager.Shutdown();
		}
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}
}
