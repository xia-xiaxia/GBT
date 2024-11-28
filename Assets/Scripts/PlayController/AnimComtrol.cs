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
        animator.SetBool("isHit",playerMovement.isHit);
    }
}

