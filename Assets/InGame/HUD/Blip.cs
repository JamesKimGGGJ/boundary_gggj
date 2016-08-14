using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Blip : MonoBehaviour {

    public GameObject target;
    public int pid;
    public float size;

    RectTransform myRectTransform;
    Map m;

    void Awake()
    {
        Player.OnDie += OnPlayerDie;
    }

    // Use this for initialization
    void Start () {
        m = GameObject.Find("Main Camera").GetComponent<Map>();
        myRectTransform = GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if(target != null)
        {
            ShowBlip();
        }
	}

    void ShowBlip()
    {
        Vector3 newP = m.cam.WorldToScreenPoint(target.transform.position);

        Vector3 stageBorder = new Vector3(Screen.width, Screen.height, 0);
        myRectTransform.localScale = new Vector2(size, size);

        newP -= stageBorder / 2;

        if (newP.x > m.map.max.x || newP.x < m.map.min.x
         || newP.y > m.map.max.y || newP.y < m.map.min.y)
        {
            GetComponent<Image>().enabled = true;
        }
        else
        {
            GetComponent<Image>().enabled = false;
        }


        if (newP != Vector3.zero)
        {
            float tilt = Mathf.Atan(newP.y / newP.x + 0.1f);

            if(newP.x > 0)
                myRectTransform.localRotation = Quaternion.Euler(0, 0, (tilt * 180) / Mathf.PI + 90f);
            else
                myRectTransform.localRotation = Quaternion.Euler(0, 0, (tilt * 180) / Mathf.PI - 90f);
        }

        Debug.Log(newP);
        newP = m.MoveInside(newP);
        myRectTransform.localPosition = newP;
    }

    void OnPlayerDie(int netId)
    {
        GameObject[] playerGameObject = GameObject.FindGameObjectsWithTag("Player");
        bool deadCheck = true;
        foreach(var obj in playerGameObject)
        {
            if (obj.GetComponent<NetworkIdentity>().clientAuthorityOwner.connectionId == pid)
                deadCheck = false;
        }

        if (deadCheck)
        {
            Destroy(gameObject);
        }
    }
}
