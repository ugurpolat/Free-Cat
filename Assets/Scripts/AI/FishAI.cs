using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAI : MonoBehaviour
{

    public float jumpSpeed;
    public float delay;
    Rigidbody2D rb;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        FishJump();
        Destroy(gameObject, delay);
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.y > 0)
        {
            sr.flipY = false;
        }
        else
        {
            sr.flipY = true;
        }

    }
    public void FishJump()
    {
        rb.AddForce(new Vector2(0, jumpSpeed));
    }
}
