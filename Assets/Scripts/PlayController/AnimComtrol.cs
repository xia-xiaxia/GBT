using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlip : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>(); // ��ȡ PlayerMovement �ű�
        spriteRenderer = GetComponent<SpriteRenderer>();  // ��ȡ SpriteRenderer ���
    }

    void Update()
    {
        // �ж���ҵ��ƶ�����
        if (playerMovement.moveDirection.x > 0)
        {
            // �����ƶ���ȷ����ҳ���
            spriteRenderer.flipX = false;
        }
        else if (playerMovement.moveDirection.x < 0)
        {
            // �����ƶ�����ת��Ҿ���
            spriteRenderer.flipX = true;
        }
    }
}

