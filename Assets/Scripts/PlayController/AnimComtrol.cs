using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private string animation;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();  
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Flip();
        AnimChange();
    }

    private void Flip()
    {
        // 判断玩家的移动方向
        if (playerMovement.direction.x > 0)
        {
            // 向右移动，确保玩家朝右
            spriteRenderer.flipX = false;
        }
        else if (playerMovement.direction.x < 0)
        {
            // 向左移动，翻转玩家精灵
            spriteRenderer.flipX = true;
        }
    }

    private void AnimChange()
    {
        animator.SetBool("isMoving", playerMovement.isMoving);
        animator.SetBool("isHit",playerMovement.isHit);
    }
}

