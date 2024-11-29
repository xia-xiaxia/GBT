using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private string animation;
    bool isFlip;
    bool isBack;
    bool isFront;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();  
        animator = GetComponent<Animator>();
        isFlip = false;
        isBack = false;
    }

    void Update()
    {
        DIR();
        AnimChange();
    }

    private void DIR()
    {
        // 判断玩家的移动方向
        if (playerMovement.direction == Vector2.left)
        {
            isFront = false;
            isFlip = true;
        }
        else if (playerMovement.direction == Vector2.right)
        {
            isFront = false;
            isFlip = false;
        }
        else if (playerMovement.direction == Vector2.up)
        {
            isFront = true;
            isBack = true;
        }
        else if (playerMovement.direction == Vector2.down)
        {
            isFront = true;
            isBack = false;
        }
    }

    private void AnimChange()
    {
        animator.SetBool("isMoving", playerMovement.isWalk);
        animator.SetBool("isFlip", isFlip);
        animator.SetBool("isBack", isBack);
        animator.SetBool("isFront", isFront);
    }
}

