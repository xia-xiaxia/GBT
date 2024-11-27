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
        playerMovement = GetComponent<PlayerMovement>(); // ��ȡ PlayerMovement �ű�
        spriteRenderer = GetComponent<SpriteRenderer>();  // ��ȡ SpriteRenderer ���
    }

    void Update()
    {
        Flip();
        AnimChange();
    }

    private void Flip()
    {
        // �ж���ҵ��ƶ�����
        if (playerMovement.direction.x > 0)
        {
            // �����ƶ���ȷ����ҳ���
            spriteRenderer.flipX = false;
        }
        else if (playerMovement.direction.x < 0)
        {
            // �����ƶ�����ת��Ҿ���
            spriteRenderer.flipX = true;
        }
    }

    private void AnimChange()
    {
        animator.SetBool("isMoving", playerMovement.isMoving);

    }
}

