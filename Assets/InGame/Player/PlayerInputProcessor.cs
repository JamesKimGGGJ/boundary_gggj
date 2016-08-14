﻿using UnityEngine;

public class PlayerInputProcessor : MonoBehaviour
{
    public Player player;
    public Rigidbody2D rb;
    public float force = 30;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public Vector2 GetMoveInput()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        return new Vector2(x, y);
    }

    bool GetFireInput()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    void Update()
    {
        if (GetFireInput()) player.CmdRequestFire();
    }

    void FixedUpdate()
    {
        FixedUpdateTransform();
    }

    void FixedUpdateTransform()
    {
        var move = GetMoveInput();
        var f = move * force;
        rb.AddForce(f);

        var v = rb.velocity;
        if (Mathf.Abs(rb.angularVelocity) > 50)
            return;

        var newAngle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.localEulerAngles = new Vector3(0, 0, newAngle);
    }
}
