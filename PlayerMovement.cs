/*
 * Script by Chris McVickar AKA Lotkey/Synth Chris
 * Github: https://github.com/lotkey
 * Website: https://synthchrismusic.wixsite.com/music
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D body;
    public PlayerCombat player;
    #region Animator
    public bool wasFacingRight = true;
    #endregion
    #region Controls
    KeyCode LEFT = KeyCode.A;
    KeyCode RIGHT = KeyCode.D;
    KeyCode JUMP = KeyCode.W;
    KeyCode SLIDE = KeyCode.LeftShift;
    #endregion
    #region Movement Booleans
    public bool isFacingRight = true;
    public bool isWalking = false;
    public bool isSliding = false;
    public bool isOnGround = true;
    public bool isJumping = false;
    public bool isLeaping = false;
    public bool isWallJumping = false;
    public bool isInWallJumpCooldown = false;
    public bool isWallClinging = false;
    public bool isWallClimbing = false;
    public bool isRecoiling = false;
    #endregion
    #region Movement Floats
    public float walkSpeed = 10f;
    public float slideForce = 1500f;
    public float jumpForce = 2500f;
    public float wallClingForce = 3000f;
    public float wallClimbForce = 10f;
    #endregion
    #region Movement Cooldowns
    private float wallClimbTime = .5f;
    private float wallClimbEndTime = 0f;
    private float wallClingTime = .5f;
    private float wallClingEndTime = 0f;
    private float wallJumpCooldownTime = .25f; // (int)(50 * 1.5);
    private float wallJumpCooldownEndTime = 0f;
    private float recoilCooldownTime = 1.0f;
    private float recoilCooldownEndTime = 0f;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
        isOnGround = hit.collider.gameObject.tag == "Ground" && transform.position.y - hit.point.y <= 1.31;
        wasFacingRight = isFacingRight;

        if (isWallClinging)
        {
            WallCling();
        }
        else if (isWallClimbing)
        {
            WallClimb();
        }
        else if (isInWallJumpCooldown)
        {
            WallJump();
        }

        if (isRecoiling)
        {
            Recoil(false);
        }

        if ((Input.GetKeyDown(SLIDE) && isWalking && isOnGround) || isSliding) // Slide
        {
            Slide();
        }
        
        if (Input.GetKeyDown(JUMP) && isOnGround && !isSliding) // Jump
        {
            Jump();
        }
        else if (Input.GetKeyDown(JUMP) && isOnGround && isSliding) // Leap
        {
            Leap();
        }
        else if(Input.GetKeyDown(JUMP) && (isWallClinging || isWallClimbing)) // Wall Jump
        {
            WallJump();
        }
        
        if (Input.GetKey(LEFT) && !isSliding && !isInWallJumpCooldown && !isRecoiling) // Walking
        {
            WalkLeft();
        }
        else if (Input.GetKey(RIGHT) && !isSliding && !isInWallJumpCooldown && !isRecoiling) // Walking
        {
            WalkRight();
        }
        else if ((Input.GetKeyUp(LEFT) || Input.GetKeyUp(RIGHT)) && !isSliding && !isLeaping && !isRecoiling) // Stop Walking
        {
            Idle();
        }
    }

    private void WalkLeft()
    {
        isFacingRight = false;
        Vector2 velocity = body.velocity;
        velocity.x = -walkSpeed;
        body.velocity = velocity;
        isWalking = true;
    }

    private void WalkRight()
    {
        isFacingRight = true;
        Vector2 velocity = body.velocity;
        velocity.x = walkSpeed;
        body.velocity = velocity;
        isWalking = true;
    }

    private void Idle()
    {
        Vector2 velocity = body.velocity;
        velocity.x = 0;
        body.velocity = velocity;
        isWalking = false;
    }

    private void Slide()
    {
        if (isSliding)
        {
            if(body.velocity.x == 0)
            {
                isSliding = false;
                Idle();
            }
        }
        else
        {
            isSliding = true;
            if (isFacingRight)
            {
                body.AddForce(Vector2.right * slideForce);
            }
            else
            {
                body.AddForce(Vector2.left * slideForce);
            }

        }
    }

    private void Jump()
    {
        isRecoiling = false;
        isOnGround = false;
        body.AddForce(Vector2.up * jumpForce);
        isJumping = true;
        isSliding = false;
    }

    private void Leap()
    {
        Jump();
        isLeaping = true;
        isSliding = false;
    }

    private void HitGround()
    {
        isOnGround = true;
        isJumping = false;
        isLeaping = false;
        isWallClinging = false;
        isWallClimbing = false;
        isWallJumping = false;
        if (Input.GetKey(SLIDE))
        {
            Slide();
        }
    }

    private void HitWall()
    {
        isSliding = false;
        if (!isOnGround)
        {
            if (isLeaping) // Wall Climb
            {
                WallClimb();
            }
            else // Wall Cling
            {
                WallCling();
            }
        }
    }

    private void WallJump()
    {
        if (!isInWallJumpCooldown)
        {
            isWallClimbing = false;
            isWallClinging = false;
            isWallJumping = true;
            isFacingRight = !isFacingRight;
            isInWallJumpCooldown = true;
            wallJumpCooldownEndTime = Time.time + wallJumpCooldownTime;
            Vector2 velocity = body.velocity;
            velocity.x = 0;
            velocity.y = 0;
            body.velocity = velocity;
            if (isFacingRight)
            {
                body.AddForce(Vector2.right * jumpForce);
            }
            else
            {
                body.AddForce(Vector2.left * jumpForce);
            }
            body.AddForce(Vector2.up * jumpForce * .75f);
        }
        else
        {
            if(Time.time >= wallJumpCooldownEndTime)
            {
                isInWallJumpCooldown = false;
            }
        }
    }

    private void WallCling()
    {
        isJumping = false;
        if (!isWallClinging)
        {
            Vector2 velocity = body.velocity;
            velocity.x = 0;
            velocity.y = 0;
            body.velocity = velocity;
            isWallClinging = true;
            wallClingEndTime = Time.time + wallClingTime;
        }
        else
        {
            RaycastHit2D hit;
            if (isFacingRight)
            {
                hit = Physics2D.Raycast(transform.position, Vector2.right);
            }
            else
            {
                hit = Physics2D.Raycast(transform.position, Vector2.left);
            }
            float distance = Mathf.Abs(hit.point.x - transform.position.x);
            bool isOnWall = distance <= .5f;

            if(Time.time >= wallClingEndTime || (isFacingRight && Input.GetKey(LEFT)) || (!isFacingRight && Input.GetKey(RIGHT)) || !isOnWall)
            {
                isWallClinging = false;
            }
            else
            {
                body.AddForce(Vector2.up * wallClingForce * Time.deltaTime);
            }
        }
    }

    private void WallClimb()
    {
        isJumping = false;
        isLeaping = false;
        if (!isWallClimbing)
        {
            isWallClimbing = true;
            isWallClinging = false;
            wallClimbEndTime = Time.time + wallClimbTime;
        }
        else
        {
            RaycastHit2D hit;
            if (isFacingRight)
            {
                hit = Physics2D.Raycast(transform.position, Vector2.right);
            }
            else
            {
                hit = Physics2D.Raycast(transform.position, Vector2.left);
            }
            float distance = Mathf.Abs(hit.point.x - transform.position.x);
            bool isOnWall = distance <= .5f;

            if (Time.time >= wallClimbEndTime)
            {
                isWallClimbing = false;
                WallCling();
            }
            else if ((isFacingRight && Input.GetKey(LEFT)) || (!isFacingRight && Input.GetKey(RIGHT)) || !isOnWall)
            {
                isWallClimbing = false;
            }
            else
            {
                body.AddForce(Vector2.up * wallClimbForce * Time.deltaTime);
            }
        }
    }

    private void Recoil(bool isRight)
    {
        if (!isRecoiling)
        {
            Vector2 velocity = body.velocity;
            velocity.x = 0;
            body.velocity = velocity;
            isRecoiling = true;
            recoilCooldownEndTime = Time.time + recoilCooldownTime;
            if (isRight)
            {
                body.AddForce(Vector2.right * jumpForce / 2f);
            }
            else
            {
                body.AddForce(Vector2.left * jumpForce / 2f);
            }
        }
        else
        {
            if (Time.time >= recoilCooldownEndTime)
            {
                isRecoiling = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Ground")
        {
            HitGround();
        }

        if (collision.collider.gameObject.tag == "Wall")
        {
            HitWall();
        }

        if (collision.collider.gameObject.tag == "AlienBlob")
        {
            Enemy enemy = collision.collider.GetComponent<Enemy>();
            if (!isOnGround)
            {
                enemy.Die();
                body.AddForce(Vector2.up * jumpForce / 2f);
            }
            else
            {
                player.TakeDamage(enemy.attackDamage);
                if (enemy.transform.position.x > body.position.x)
                {
                    Recoil(false);
                }
                else
                {
                    Recoil(true);
                }
            }
        }

    }
}