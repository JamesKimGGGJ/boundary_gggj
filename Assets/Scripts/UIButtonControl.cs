using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class UIButtonControl : MonoBehaviour {
    public Image[] peoples;
    public GameObject peopleShadow;
    public bool Ready { get; set; }
    public Lobby lobby;
    public bool AllReady { get; set; }
    public bool start;
    public string startStr;
    public string readyStr;
    public string secStr_0;
    public string secStr_1;
    public Transform cameraSecondPosition;

    public GameObject creditPopUp;
    public Button creditButton;

    Text text;
    Button button;
    public Messanger msg;

    void Start() {
		lobby = FindObjectOfType<Lobby> ();
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
                    text.text = startStr;
                    button.interactable = AllReady;
                }
                else {
                    text.text = readyStr;
                    button.interactable = false;
                }
            }
            switch (lobby.numPlayers > 0 ? lobby.numPlayers : msg.num) {
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
            msg.RpcMeg(0,0);
        }
        else {
            Camera.main.GetComponent<LobbyCameraPan>().enabled = false;
            DOTween.Init();
            Camera.main.transform.DOMove(cameraSecondPosition.position,1).SetEase(Ease.OutCubic);
            Camera.main.transform.DORotate(cameraSecondPosition.rotation.eulerAngles,1)
                .OnComplete(() => Camera.main.GetComponent<LobbyCameraPan>().enabled = true);
            lobby.FindInternetMatch();
            button.interactable = false;
            peopleShadow.SetActive(true);
        }
    }

    public IEnumerator startGame (){
        int i = Lobby.playerId;
        int remain;
        start = true;
        button.interactable = false;
        lobby.closeMatch();
        for (remain = 5; remain > 0; remain--) {
            text.text = secStr_0 + remain + secStr_1;
            yield return new WaitForSeconds(1);
        }

        lobby.ServerChangeScene(lobby.playScene);
    }

    public void CreditOn()
    {
        creditPopUp.SetActive(true);
    }

    public void CreditOff()
    {
        creditPopUp.SetActive(false);
    }

    public void CreditButtonInactive()
    {
        creditButton.interactable = false;
    }
}
