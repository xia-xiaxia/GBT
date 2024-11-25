using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector2 moveDirection; // ����ƶ�����
    private Vector2 targetPosition; // Ŀ��λ��
    public float moveSpeed = 5f; // �ƶ��ٶ�
    public float gridSize = 1f; // ÿ��Ĵ�С

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = transform.position; // ��ʼλ�þ���Ŀ��λ��
    }

    void Update()
    {
        // ��ȡ��ҵ��ƶ�����
        float moveX = 0;
        float moveY = 0;

        if (Input.GetKey(KeyCode.W))
            moveY++;
        if (Input.GetKey(KeyCode.S))
            moveY--;
        if (Input.GetKey(KeyCode.A))
            moveX--;
        if (Input.GetKey(KeyCode.D))
            moveX++;

        // �����ƶ�����
        moveDirection = new Vector2(moveX, moveY).normalized; // ��֤ÿ֡���ƶ������ǵ�λ����

        // ����Ŀ��λ�ã���һ������꣩
        if (moveDirection != Vector2.zero)
        {
            // ����Ŀ��λ�ã�������ƶ�����ǰ���ӵ���һ��
            targetPosition = new Vector2(Mathf.Round(transform.position.x / gridSize) * gridSize + moveDirection.x * gridSize,
                                         Mathf.Round(transform.position.y / gridSize) * gridSize + moveDirection.y * gridSize);
        }
    }

    void FixedUpdate()
    {
        // ʹ�ò�ֵƽ���ƶ���Ŀ��λ��
        transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
    }
}
