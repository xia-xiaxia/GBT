using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 targetPosition; // 目标位置
    public float moveSpeed; // 移动速度
    public float gridSize = 1f; // 每格的大小
    public bool isMoving = false; // 是否正在移动
    public Vector2 direction; // 玩家当前的移动方向

    private Vector3 lastPosition;
    private bool isTrans;

    public bool isPush = false;

    public bool isPossessed ;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = transform.position; // 初始目标位置就是玩家当前位置
        isPossessed = false;
    }

    void Update()
    {
        if (isPossessed)
        {
            Debug.Log("cnmlgb");
            return;
        }
        // 如果玩家正在移动，就不接受新的输入
        if (isMoving)
            return;
        // 获取玩家的移动输入
        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector2.up; // 向上
            StartMove();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            direction = Vector2.down; // 向下
            StartMove();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            direction = Vector2.left; // 向左
            StartMove();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector2.right; // 向右
            StartMove();
        }
    }

    void FixedUpdate()
    {
        // 如果正在移动，使用插值平滑过渡到目标位置
        if (isMoving)
        {
            // 使用 Lerp 来平滑过渡到目标位置
            transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);

            // 判断是否到达目标位置，若到达，停止移动
            if (Vector2.Distance(transform.position, targetPosition) < 0.05f)
            {
                transform.position = targetPosition; // 精确到目标位置
                isMoving = false; // 停止移动
            }
        }
    }

    // 启动移动过程
    private void StartMove()
    {
        // 计算目标位置（玩家要到达的格子中心，即 (n + 0.5, m + 0.5)）
        targetPosition = new Vector2(Mathf.Floor(transform.position.x / gridSize) * gridSize + 0.5f * gridSize + direction.x * gridSize,
                                     Mathf.Floor(transform.position.y / gridSize) * gridSize + 0.5f * gridSize + direction.y * gridSize);
        isMoving = true; // 标记为正在移动
    }

    


    void isMovingorNot()
    {
        // 每帧检查物体是否移动
        if (transform.position != lastPosition)
        {
            isTrans = true;  // 如果位置发生变化，认为物体正在移动
        }
        else
        {
            isTrans = false; // 如果位置没有变化，认为物体没有移动
        }

        // 更新上一帧的位置
        lastPosition = transform.position;
    }

    
}
