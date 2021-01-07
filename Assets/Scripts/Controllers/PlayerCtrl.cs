using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [Tooltip("This is a positive integer which speeds up the player movement")]
    public int speedBoost;
    public float jumpSpeed;
    public bool isJumping;
    public bool isGrounded, canDoubleJump;
    public Transform feet;
    public float feetRadius;
    public LayerMask whatIsGround;
    public float boxWidth;
    public float boxHeight;
    public float delayForDoubleJump;
    public bool leftPressed, rightPressed;
    public Transform leftBulletSpawnPos, rightBulletSpawnPos;
    public GameObject leftBullet, rightBullet;
    public bool SFXOn;
    public bool canFire;
    public bool isStuck;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    public GameObject garbageCtrl;

    void Awake()
    {
        if (PlayerPrefs.HasKey("CPX"))
        {
            transform.position = new Vector3(PlayerPrefs.GetFloat("CPX"), PlayerPrefs.GetFloat("CPY"),transform.position.z);
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    
    void Update() 
    {
        //isGrounded = Physics2D.OverlapCircle(feet.position,feetRadius,whatIsGround);
        isGrounded = Physics2D.OverlapBox(new Vector2(feet.position.x, feet.position.y), new Vector2(boxWidth, boxHeight), 360.0f, whatIsGround);
        
        float playerSpeed = Input.GetAxisRaw("Horizontal");
        
        playerSpeed *= speedBoost;
        if (playerSpeed != 0)
        {
            MoveHorizontal(playerSpeed);
        }
        else
        {
            StopMoving();
        }
        if (Input.GetButtonDown("Jump"))
            Jump();
        if (Input.GetButtonDown("Fire1"))
        {
            FireBullets();
        }
        ShowFalling();

        if (leftPressed)
        {
            MoveHorizontal(-speedBoost);
        }
        if (rightPressed)
        {
            MoveHorizontal(speedBoost);
        }
    }
    private void FireBullets()
    {
        if (canFire)
        {
            if (sr.flipX)
            {
                Instantiate(leftBullet, leftBulletSpawnPos.position, Quaternion.identity);
            }
            if (!sr.flipX)
            {
                Instantiate(rightBullet, rightBulletSpawnPos.position, Quaternion.identity);
            }

            AudioCtrl.instance.FireBullets(gameObject.transform.position);
        }
        
    }
    private void OnDrawGizmos()
    {
        
        Gizmos.DrawWireCube(feet.position, new Vector3(boxWidth,boxHeight,0));
    }
    void MoveHorizontal(float playerSpeed)
    {
        rb.velocity = new Vector2(playerSpeed, rb.velocity.y);
        if (rb.velocity.x > 0)
        {
            sr.flipX = false;
        }
        else if (rb.velocity.x < 0)
        {
            sr.flipX = true;
        }

        if (!isJumping)
        {
            anim.SetInteger("State", 1);
        }
        
       
    }
    void StopMoving()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        if (!isJumping)
        {
            anim.SetInteger("State", 0);
        }
        
    }
    void ShowFalling()
    {
        if (rb.velocity.y < 0)
        {
            anim.SetInteger("State", 3);
        }
    }
    void Jump()
    {
        if (isGrounded)
        {
            isJumping = true;
            rb.AddForce(new Vector2(0, jumpSpeed));
            anim.SetInteger("State", 2);
            AudioCtrl.instance.PlayerJump(gameObject.transform.position);
            Invoke("EnableDoubleJump", delayForDoubleJump);
        }
        if (canDoubleJump && !isGrounded)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, jumpSpeed));
            anim.SetInteger("State", 2);
            AudioCtrl.instance.PlayerJump(gameObject.transform.position);
            canDoubleJump = false;
        }
    }
    void EnableDoubleJump()
    {
        canDoubleJump = true;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            AudioCtrl.instance.PlayerDied(gameObject.transform.position);
            GameCtrl.instance.PlayerDiedAnimation(gameObject);
        }
        if (other.gameObject.CompareTag("BigCoin"))
        {
            AudioCtrl.instance.CoinPickup(gameObject.transform.position);
            GameCtrl.instance.UpdateCoinCount();
            SFXCtrl.instance.ShowBulletSparkle(other.gameObject.transform.position);
            Destroy(other.gameObject);
            GameCtrl.instance.UpdateScore(GameCtrl.Item.BigCoin);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Coin":
                if (SFXOn)
                {
                    SFXCtrl.instance.ShowCoinSparkle(other.gameObject.transform.position);
                    
                }
                AudioCtrl.instance.CoinPickup(gameObject.transform.position);
                GameCtrl.instance.UpdateCoinCount();
                GameCtrl.instance.UpdateScore(GameCtrl.Item.Coin);

                break;

            case "Powerup_Bullet":
                canFire = true;
                Vector3 powerupPos = other.gameObject.transform.position;
                AudioCtrl.instance.PowerUp(gameObject.transform.position);
                
                Destroy(other.gameObject);
                if (SFXOn)
                {
                    SFXCtrl.instance.ShowBulletSparkle(powerupPos);
                }
                break;
            case "Water":
                garbageCtrl.SetActive(false);
                AudioCtrl.instance.WaterSplash(gameObject.transform.position);
                SFXCtrl.instance.ShowSplash(gameObject.transform.position);
                GameCtrl.instance.PlayerDrowned(other.gameObject);
                break;
            case "Enemy":
                AudioCtrl.instance.PlayerDied(gameObject.transform.position);
                GameCtrl.instance.PlayerDiedAnimation(gameObject);
                break;
            case "BossKey":
                GameCtrl.instance.ShowLever();
                
                break;

            default:
                break;
        }

    }
    public void MobileMoveLeft()
    {
        leftPressed = true;
    }
    public void MobileMoveRight()
    {
        rightPressed = true;
    }
    public void MobileStop()
    {
        leftPressed = false;
        rightPressed = false;

        StopMoving();
    }
    public void MobileFireBUllets()
    {
        FireBullets();
    }
    public void MobileJump()
    {
        Jump();
    }
}
