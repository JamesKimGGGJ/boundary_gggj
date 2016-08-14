using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Map : MonoBehaviour {

    public GameObject blip;
    public GameObject panel;
    public GameObject targetPosition;
    public Camera cam;
    public float zoomLv;

    Transform mainTarget;
    public Rect map;

    void Awake()
    {
        Player.OnSpawn += OnPlayerSpawn;
        cam = GetComponent<Camera>();
    }

    // Use this for initialization
    void Start () {
        map = panel.GetComponent<RectTransform>().rect;
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void SetMainTarget(Transform T)
    {
        mainTarget = T;
    }

    public Vector2 MoveInside(Vector3 p)
    {
        Vector2 screenBorder = new Vector2(Screen.width, Screen.height);

        if (p.y > map.max.y)
        {
            p = new Vector3((p.x * map.max.y) / p.y, map.max.y, 0);
        }
        if(p.y < map.min.y)
        {
            p = new Vector3((p.x * map.min.y) / p.y, map.min.y, 0);
        }
        if(p.x > map.max.x)
        {
            p = new Vector3(map.max.x, (p.y * map.max.x) / p.x, 0);
        }
        if(p.x < map.min.x)
        {
            p = new Vector3(map.min.x, (p.y * map.min.x) / p.x, 0);
        }

        return p;
    }

    void OnPlayerSpawn(int pId, GameObject player)
    {
        GameObject b;
        b = Instantiate(blip, transform.position, transform.rotation) as GameObject;
        b.GetComponent<Blip>().pid = pId;
        b.GetComponent<Blip>().target = player;

        b.transform.SetParent(panel.transform);
    }
}
