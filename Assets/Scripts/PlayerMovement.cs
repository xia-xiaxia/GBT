using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector2 moveDirection;
    Transform transform;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform = rb.transform;
    }

    void Update()
    {
        float moveX = 0;
        float moveY = 0;
        if (Input.GetKeyDown(KeyCode.W))
            moveY++;
        if (Input.GetKeyDown(KeyCode.S))
            moveY--;
        if (Input.GetKeyDown(KeyCode.A))
            moveX--;
        if (Input.GetKeyDown(KeyCode.D))
            moveX++;
        Vector2 moveDirection = new Vector2(moveX, moveY).normalized;

        transform.position += new Vector3(moveX,moveY,0f);
    }

}
