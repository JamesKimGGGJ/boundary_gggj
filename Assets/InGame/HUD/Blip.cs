using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Blip : MonoBehaviour {

    public GameObject target;
    public int pId;
    public float blipSize;

    RectTransform myRectTransform;
    Map map;

    void Awake()
    {
        Player.OnDie += OnPlayerDie;
    }

    IEnumerator Start()
    {
        map = GameObject.Find("Main Camera").GetComponent<Map>();
        myRectTransform = GetComponent<RectTransform>();

        GameObject renderer = target.transform.FindChild("Renderer").gameObject;

        GameObject m_r = renderer.transform.FindChild("model_r").gameObject;
        GameObject m_b = renderer.transform.FindChild("model_b").gameObject;
        GameObject m_g = renderer.transform.FindChild("model_g").gameObject;
        GameObject m_w = renderer.transform.FindChild("model_y").gameObject;

        yield return new WaitForEndOfFrame ();

        if (m_r.activeSelf)
        {
            GetComponent<Image>().color = Color.red;
            Debug.Log("m_r");
        }
        else if (m_b.activeSelf)
        {
            GetComponent<Image>().color = Color.blue;
            Debug.Log("m_b");
        }
        else if (m_g.activeSelf)
        {
            GetComponent<Image>().color = Color.green;
            Debug.Log("m_g");
        }
        else if (m_w.activeSelf)
        {
            GetComponent<Image>().color = Color.white;
            Debug.Log("m_w");
        }

        yield return 0.0f;
    }

    void LateUpdate()
    {
        Vector3 scp = map.cam.WorldToScreenPoint(target.transform.position) - new Vector3(Screen.width, Screen.height, 0) / 2;
        Vector2 scpp = new Vector2(scp.x, scp.y);

        if(target != null)
        {
            if (map.MoveInside(scpp))
            {
                GetComponent<Image>().enabled = false;
            }
            else
            {
                GetComponent<Image>().enabled = true;
            }

            Vector2 td = SetDirection() - new Vector2(Screen.width, Screen.height) / 2;
            td *= 100;
            td = map.ReLocateBlip(td);
            myRectTransform.localPosition = td;

            RotateBlip(td);
        }
    }

    Vector2 SetDirection()
    {
        Vector3 center = map.camTarget.transform.position;
        Vector3 dir = target.transform.position - center;
        ResizeBlip(dir);
        dir = (Vector3.Normalize(dir));
        Vector3 targetdir = map.cam.WorldToScreenPoint(dir);
        myRectTransform.localPosition = targetdir - new Vector3(Screen.width, Screen.height, 0) / 2;
        return new Vector2(targetdir.x, targetdir.y);
    }

    void RotateBlip(Vector2 p)
    {
        if (p != Vector2.zero)
        {
            float tilt = Mathf.Atan(p.y / p.x + 0.1f);

            if (p.x > 0)
                myRectTransform.localRotation = Quaternion.Euler(0, 0, (tilt * 180) / Mathf.PI + 90f);
            else
                myRectTransform.localRotation = Quaternion.Euler(0, 0, (tilt * 180) / Mathf.PI - 90f);
        }
    }

    void ResizeBlip(Vector2 p)
    {
        if (p == Vector2.zero)
            return;

        float mag = p.magnitude;
        myRectTransform.localScale = new Vector3(blipSize, blipSize, 0) / mag;
    }

    void OnPlayerDie(int netId)
    {
        if(target.GetComponent<NetworkIdentity>().clientAuthorityOwner.connectionId == netId)
        { 
            Destroy(gameObject);
        }
    }

    /*
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
    */
}
