using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlip : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>(); // 获取 PlayerMovement 脚本
        spriteRenderer = GetComponent<SpriteRenderer>();  // 获取 SpriteRenderer 组件
    }

    void Update()
    {
        // 判断玩家的移动方向
        if (playerMovement.moveDirection.x > 0)
        {
            // 向右移动，确保玩家朝右
            spriteRenderer.flipX = false;
        }
        else if (playerMovement.moveDirection.x < 0)
        {
            // 向左移动，翻转玩家精灵
            spriteRenderer.flipX = true;
        }
    }
}

