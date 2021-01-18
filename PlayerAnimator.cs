/*
 * Script by Chris McVickar AKA Lotkey/Synth Chris
 * Github: https://github.com/lotkey
 * Website: https://synthchrismusic.wixsite.com/music
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    public PlayerCombat combat;
    public PlayerMovement movement;

    #region Strings
    private string isIdle = "isIdle";
    private string isWalking = "isWalking";
    private string isSliding = "isSliding";
    private string isJumping = "isJumping";
    private string isLeaping = "isLeaping";
    private string isWallJumping = "isWallJumping";
    private string isWallClinging = "isWallClinging";
    private string isWallClimbing = "isWallClimbing";
    private string isRecoiling = "isRecoiling";
    private string isFalling = "isFalling";
    #endregion
    private void Start()
    {
    }

    private void Update()
    {
        if (movement.wasFacingRight != movement.isFacingRight)
        {
            Vector2 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        animator.SetBool(isIdle, false);
        animator.SetBool(isWalking, false);
        animator.SetBool(isSliding, false);
        animator.SetBool(isJumping, false);
        animator.SetBool(isLeaping, false);
        animator.SetBool(isWallJumping, false);
        animator.SetBool(isWallClinging, false);
        animator.SetBool(isWallClimbing, false);
        animator.SetBool(isRecoiling, false);

        if (movement.isWallClinging)
        {
            animator.SetBool(isWallClinging, true);
        }
        else if (movement.isWalking)
        {
            animator.SetBool(isWalking, true);
        }
        else if (movement.isSliding)
        {
            animator.SetBool(isSliding, true);
        }
        else if (movement.isJumping)
        {
            animator.SetBool(isSliding, true);
        }
        else if (movement.isLeaping)
        {
            animator.SetBool(isLeaping, true);
        }
        else if (movement.isWallJumping)
        {
            animator.SetBool(isWallJumping, true);
        }
        else if (movement.isWallClimbing)
        {
            animator.SetBool(isWallClimbing, true);
        }
        else if (movement.isRecoiling)
        {
            animator.SetBool(isRecoiling, true);
        }
        else
        {
            animator.SetBool(isIdle, true);
        }
    }
}