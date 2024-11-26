using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Box : MonoBehaviour
{
    public PlayerMovement PM;
    private Transform box;
    private Collider2D col;    // Box 的 Collider
    private bool isCollision = false;  // 判断是否可以碰撞
    private bool isMoving = false;
    [SerializeField] private float currentSpeed;
    public float slowSpeed;
    public float normalSpeed;
    public float quickSpeed;
    private float gridSize = 1f;
    private Vector3 targetPosition;
    private Vector2 boxDir;

    void Start()
    {
        box = GetComponent<Transform>();
        col = GetComponent<Collider2D>();
        currentSpeed = normalSpeed;
    }

    void Update()
    {
        PM.moveSpeed = currentSpeed;
        DirJudge();  // 判断玩家是否朝向 Box 移动
    }

    // 判断玩家是否朝向 Box 移动
    void DirJudge()
    {
        Vector3 dirToBox = box.position - PM.transform.position;
        float angle = Vector3.Angle(PM.direction, dirToBox.normalized);

        if (angle < 45f)
        {
            isCollision = true; // 允许推动
        }
        else
        {
            isCollision = false; // 不允许推动
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Player")
        {
            isCollision = true; // 标记开始碰撞，开始对齐
            boxDir = PM.direction;
            boxMove();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (isCollision && collision.collider.name == "Player")
        {
            currentSpeed = slowSpeed; // 玩家移动速度减慢
        }
    }

    // 碰撞退出
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.name == "Player")
        {
            // 恢复玩家的移动速度，并清除碰撞状态
            PM.moveSpeed = normalSpeed;
            isCollision = false;
            isMoving = false;
        }
    }

    void FixedUpdate()
    {
        // 如果正在移动，使用插值平滑过渡到目标位置
        if (isMoving)
        {
            // 使用 Lerp 来平滑过渡到目标位置
            transform.position = Vector2.Lerp(transform.position, targetPosition, currentSpeed * Time.fixedDeltaTime);

            // 判断是否到达目标位置，若到达，停止移动
            if (Vector2.Distance(transform.position, targetPosition) < 0.05f)
            {
                transform.position = targetPosition; // 精确到目标位置
                isMoving = false; // 停止移动
            }
        }
    }


    void boxMove()
    {
        targetPosition = new Vector2(Mathf.Floor(transform.position.x / gridSize) * gridSize + 0.5f * gridSize + boxDir.x * gridSize,
                                    Mathf.Floor(transform.position.y / gridSize) * gridSize + 0.5f * gridSize + boxDir.y * gridSize);

        isMoving = true; // 标记为正在移动
    }

    
}
