using UnityEngine;

public class StormForce : MonoBehaviour
{
    private const float stormRange = 15;
    private const float multiplier = 6.6f;
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

        if (radius > stormR)
        {
            // 폭풍 밖은 안전하다.
            return;
        }

        var f = Vector2.zero;
        var innerStormR = stormR - stormRange;
        if (radius > innerStormR)
        {
            float max = Mathf.Min(stormRange, stormR) * multiplier;
            float force = max - (multiplier * (stormR - radius));
            f = force * fDir;
        }

        rb.AddForceAtPosition(f, transform.position);
    }
}
