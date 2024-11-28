using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessedMove : MonoBehaviour
{
    private Vector2 targetPosition; // 目标位置
    public float moveSpeed = 4f; // 移动速度
    public float gridSize = 1f; // 每格的大小
    public bool isMoving = false; // 是否正在移动
    public Vector2 direction; // 玩家当前的移动方向
    public bool isHit = false;
    public PlayerMovement PM;
    public LayerMask layer;

    void Start()
    {
        targetPosition = transform.position; // 初始目标位置就是玩家当前位置
    }

    void Update()
    {
        // 如果正在移动，就忽略输入
        if (PM != null)
        {
            if (PM.isMoving) return;
        }
        else
        {
            if (isMoving)
                return;
        }

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

    private void StartMove()
    {
        // 计算目标位置（玩家要到达的格子中心，即 (n + 0.5, m + 0.5)）
        targetPosition = new Vector2(Mathf.Floor(transform.position.x / gridSize) * gridSize + 0.5f * gridSize + direction.x * gridSize,
                                     Mathf.Floor(transform.position.y / gridSize) * gridSize + 0.5f * gridSize + direction.y * gridSize);
        isMoving = true; // 标记为正在移动
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        int layer = collision.collider.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Hinder"))
        {
            isMoving = false;
            isHit = true;
            calculate();
            transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        int layer = collision.collider.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Hinder"))
        {
            calculate();
            transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        int layer = collision.collider.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Hinder"))
        {
            isHit = false;
            calculate();
            transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void calculate()
    {
        targetPosition = new Vector2(Mathf.Floor(transform.position.x / gridSize) * gridSize + 0.5f * gridSize,
                                     Mathf.Floor(transform.position.y / gridSize) * gridSize + 0.5f * gridSize);
    }
    IEnumerator recovery()
    {
        yield return new WaitForSeconds(0.2f);
        isHit = false;
    }
}
