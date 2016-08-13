﻿using UnityEngine;

public enum PlayerControlScheme { Right, Left, }

public class PlayerInputProcessor : MonoBehaviour
{
    public PlayerControlScheme scheme;
    public PlayerLocal playerLocal;
    public Rigidbody2D rb;
    public float force = 10;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void GetMoveInput(out int x, out int y)
    {
        x = 0;
        y = 0;

        if (scheme == PlayerControlScheme.Right)
        {
            if (Input.GetKey(KeyCode.LeftArrow)) x = -1;
            else if (Input.GetKey(KeyCode.RightArrow)) x = 1;
            if (Input.GetKey(KeyCode.DownArrow)) y = -1;
            else if (Input.GetKey(KeyCode.UpArrow)) y = 1;
            if (Input.GetKey(KeyCode.Comma)) playerLocal.Fire();
        }
        else
        {
            if (Input.GetKey(KeyCode.A)) x = -1;
            else if (Input.GetKey(KeyCode.D)) x = 1;
            if (Input.GetKey(KeyCode.S)) y = -1;
            else if (Input.GetKey(KeyCode.W)) y = 1;
            if (Input.GetKey(KeyCode.Space)) playerLocal.Fire();
        }
    }

    void Update()
    {
        int x, y;
        GetMoveInput(out x, out y);
        if (x == 0 && y == 0) return;

        var f = new Vector2(x, y) * force;
        rb.AddForce(f);

        var v = rb.velocity;
        var newAngle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        // var orgAngle = transform.localEulerAngles.z;
        // var nextAngle = Mathf.LerpAngle(orgAngle, newAngle, Time.deltaTime);
        transform.localEulerAngles = new Vector3(0, 0, newAngle);
    }
}
