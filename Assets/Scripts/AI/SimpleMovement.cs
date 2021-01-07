using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public float speed;

    Rigidbody2D rb;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        SetStartingDirection();
    }
    void Update()
    {
        Move();
    }
    void Move()
    {
        Vector2 temp = rb.velocity;
        temp.x = speed;
        rb.velocity = temp;
    }
    private void SetStartingDirection()
    {
        if (speed > 0)
        {
            sr.flipX = true;
        }
        else if (speed < 0)
        {
            sr.flipX = false;
        }
    }

    // Update is called once per frame
    

    
    void FlipOnCollision()
    {
        speed = -speed;
        SetStartingDirection();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            FlipOnCollision();
        }
    }
}
