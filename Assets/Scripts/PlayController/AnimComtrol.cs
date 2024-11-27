using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlip : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private string animation;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>(); // 获取 PlayerMovement 脚本
        spriteRenderer = GetComponent<SpriteRenderer>();  // 获取 SpriteRenderer 组件
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

    }
}

