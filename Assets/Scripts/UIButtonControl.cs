using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIButtonControl : MonoBehaviour {
    public Image[] peoples;
    public bool Ready { get; set; }
    public Lobby lobby;
    public bool AllReady { get; set; }
    public bool start;
    Text text;
    Button button;

    void Start() {
        text = gameObject.GetComponentInChildren<Text>();
        button = GetComponent<Button>();
        Ready = false;
        AllReady = false;
        start = false;
    }

	// Update is called once per frame
	void Update () {
        if (Ready) {
            switch (lobby.numPlayers) {
                case 0:
                    peoples[0].enabled = false;
                    peoples[1].enabled = false;
                    peoples[2].enabled = false;
                    peoples[3].enabled = false;
                    text.text = "Ready";
                    button.interactable = false;
                    break;
                case 1:
                    peoples[0].enabled = true;
                    peoples[1].enabled = false;
                    peoples[2].enabled = false;
                    peoples[3].enabled = false;
                    text.text = "Start";
                    button.interactable = false;
                    break;
                case 2:
                    peoples[0].enabled = true;
                    peoples[1].enabled = true;
                    peoples[2].enabled = false;
                    peoples[3].enabled = false;
                    if (!start)
                    {
                        text.text = "Start";
                        button.interactable = AllReady;
                    }
                    break;
                case 3:
                    peoples[0].enabled = true;
                    peoples[1].enabled = true;
                    peoples[2].enabled = true;
                    peoples[3].enabled = false;
                    if (!start)
                    {
                        text.text = "Start";
                        button.interactable = AllReady;
                    }
                    break;
                case 4:
                    peoples[0].enabled = true;
                    peoples[1].enabled = true;
                    peoples[2].enabled = true;
                    peoples[3].enabled = true;
                    if (!start)
                    {
                        text.text = "Start";
                        button.interactable = AllReady;
                    }
                    break;
            }
        }
	}

    public void startButton() {
        if (Ready)
        {
            StartCoroutine(startGame());
            start = true;
        }
        else {
            lobby.StopMatchMaker();
            lobby.FindInternetMatch();
            button.interactable = false;
        }
    }

    IEnumerator startGame (){
        int remain;
        button.interactable = false;
        for (remain = 5; remain > 0; remain--) {
            text.text = "" + remain + "초 뒤 매칭된 폭풍으로 이동합니다.";
            yield return new WaitForSeconds(1);
        }
        lobby.ServerChangeScene(lobby.playScene);
    }
}
