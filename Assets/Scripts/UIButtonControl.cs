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
    Messanger msg;

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
            if (!start) {
                if (lobby.numPlayers > 0)
                {
                    text.text = "Start";
                    button.interactable = lobby.numPlayers > 1 ? AllReady : false;
                }
                else {
                    text.text = "Ready";
                    button.interactable = false;
                }
            }
            switch (lobby.numPlayers > 0 ? lobby.numPlayers : GameObject.Find("Messanger").GetComponent<Messanger>().num) {
                case 0:
                case 1:
                    peoples[0].enabled = true;
                    peoples[1].enabled = false;
                    peoples[2].enabled = false;
                    peoples[3].enabled = false;
                    break;
                case 2:
                    peoples[0].enabled = true;
                    peoples[1].enabled = true;
                    peoples[2].enabled = false;
                    peoples[3].enabled = false;
                    break;
                case 3:
                    peoples[0].enabled = true;
                    peoples[1].enabled = true;
                    peoples[2].enabled = true;
                    peoples[3].enabled = false;
                    break;
                case 4:
                    peoples[0].enabled = true;
                    peoples[1].enabled = true;
                    peoples[2].enabled = true;
                    peoples[3].enabled = true;
                    break;
            }
        }
	}

    public void startButton() {
        if (Ready)
        {
            StartCoroutine(startGame());
            GameObject.Find("Messanger").GetComponent<Messanger>().RpcMeg(0,0);
            
        }
        else {
            lobby.FindInternetMatch();
            button.interactable = false;
        }
    }

    public IEnumerator startGame (){
        int remain;
        start = true;
        button.interactable = false;
        for (remain = 5; remain > 0; remain--) {
            text.text = "" + remain + "초 뒤 매칭된 폭풍으로 이동합니다.";
            yield return new WaitForSeconds(1);
        }
        lobby.ServerChangeScene(lobby.playScene);
    }
}
