using UnityEngine;

public class StormForce : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var pos = (Vector2)transform.position;
        var fDir = -pos.normalized;
        var radius = pos.magnitude;
        var stormR = GameGlobalVar.stormRadius;

        var f = Vector2.zero;
        var innerStormR = 0.7f * stormR;
        if (radius < innerStormR)
        {
            f = 2 * fDir;
        }
        else
        {
            f = (2 + 10 * (radius - innerStormR)) * fDir;
        }

        rb.AddForceAtPosition(f, transform.position);
    }
}
