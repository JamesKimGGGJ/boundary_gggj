using UnityEngine;

public class LobbyCameraPan : MonoBehaviour
{
    private Camera cam;
    private Vector3 panPosOrigin;
    private Quaternion panRotOrigin;

    void OnEnable()
    {
        cam = GetComponent<Camera>();
        panPosOrigin = transform.position;
        panRotOrigin = transform.rotation;
    }

    void Update()
    {
        var mousePos = Input.mousePosition;
        var mouseRel = new Vector2(mousePos.x / Screen.width - 0.5f, mousePos.y / Screen.height - 0.5f);
        cam.transform.position = Vector3.Lerp(cam.transform.position, panPosOrigin + (Vector3)mouseRel * 0.3f, Time.deltaTime);

        cam.transform.LookAt(cam.transform.TransformPoint(new Vector3(mouseRel.x, mouseRel.y, 100)));
        var angle = cam.transform.rotation;
        angle = Quaternion.Lerp(angle, panRotOrigin, Time.deltaTime);
        cam.transform.rotation = angle;
    }
}
