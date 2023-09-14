using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontalInput, verticalInput);

       
        MovePlayer(movement);
    }

    void MovePlayer(Vector2 movement)
    {
        if (movement.sqrMagnitude > 1)
        {
            movement.Normalize();
        }

        rb.velocity = movement * speed;
    }
}

