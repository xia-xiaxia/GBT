using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector2 moveDirection; // 玩家移动方向
    private Vector2 targetPosition; // 目标位置
    public float moveSpeed = 5f; // 移动速度
    public float gridSize = 1f; // 每格的大小

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = transform.position; // 初始位置就是目标位置
    }

    void Update()
    {
        // 获取玩家的移动输入
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

        // 设置移动方向
        moveDirection = new Vector2(moveX, moveY).normalized; // 保证每帧的移动方向是单位向量

        // 计算目标位置（下一格的坐标）
        if (moveDirection != Vector2.zero)
        {
            // 计算目标位置，将玩家移动到当前格子的下一格
            targetPosition = new Vector2(Mathf.Round(transform.position.x / gridSize) * gridSize + moveDirection.x * gridSize,
                                         Mathf.Round(transform.position.y / gridSize) * gridSize + moveDirection.y * gridSize);
        }
    }

    void FixedUpdate()
    {
        // 使用插值平滑移动到目标位置
        transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
    }
}
