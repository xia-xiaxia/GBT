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
    [SerializeField] private float currentSpeed;
    public float slowSpeed;
    public float normalSpeed;
    public float quickSpeed;
    private float gridSize = 1f;
    private Vector3 targetPosition;

    private Vector3 lastPosition;  
    public bool isMoving = false;  // 表示物体是否正在移动

    private bool isCollidingWithPlayer = false; // 表示是否正在与玩家发生碰撞

    void Start()
    {
        box = GetComponent<Transform>();
        col = GetComponent<Collider2D>();
        currentSpeed = normalSpeed;
        lastPosition = transform.position;
    }

    void Update()
    {
        PM.moveSpeed = currentSpeed;
        DirJudge();  // 判断玩家是否朝向 Box 移动
        AlignToGrid();
        if (isCollidingWithPlayer)
            isMovingorNot();
        Debug.Log(isCollidingWithPlayer);
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
        if (isCollision && collision.collider.name == "Player")
        {
            box.SetParent(collision.transform); // 设置 Box 为玩家的子物体
            isCollidingWithPlayer = true; // 标记开始碰撞，开始对齐
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
        // 如果与玩家发生碰撞退出
        if (collision.collider.name == "Player")
        {
            // 恢复玩家的移动速度，并解除 Box 的父物体关系
            ResetBox();
        }
    }

    void AlignToGrid()
    {
        float moveSpeed = 3f;
        Vector2 currentPosition = transform.position;

        // 计算格子的中心点
        float alignedX = Mathf.Floor(currentPosition.x / gridSize) * gridSize + 0.5f * gridSize;
        float alignedY = Mathf.Floor(currentPosition.y / gridSize) * gridSize + 0.5f * gridSize;

        // 判断是否已经在格子中心
        bool isAtGridCenter = Mathf.Abs(currentPosition.x - alignedX) < 0.1f && Mathf.Abs(currentPosition.y - alignedY) < 0.1f;

        if (isCollidingWithPlayer)
        {
            if (isAtGridCenter)
            {
                // 如果已经在格子中心，考虑玩家朝向的方向，计算下一个格子的中心点
                targetPosition = new Vector3(
                    alignedX + PM.direction.x * gridSize,  // 计算玩家朝向下一个格子
                    alignedY + PM.direction.y * gridSize,
                    0f
                );
            }
            else
            {
                // 如果不在格子中心，直接跳过当前格子，计算玩家朝向的下一个格子
                targetPosition = new Vector3(
                    alignedX + PM.direction.x * gridSize,  // 计算玩家朝向下一个格子
                    alignedY + PM.direction.y * gridSize,
                    0f
                );
            }
        }
        else
            targetPosition = new Vector3(alignedX, alignedY, 0f);

        // 使用 Lerp 平滑过渡到目标位置
        if (transform.position != targetPosition)
        {
            transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        }
    }

    void isMovingorNot()
    {
        // 每帧检查物体是否移动
        if (transform.position != lastPosition)
        {
            isMoving = true;  // 如果位置发生变化，认为物体正在移动
        }
        else
        {
            isMoving = false; // 如果位置没有变化，认为物体没有移动
        }

        // 更新上一帧的位置
        lastPosition = transform.position;
    }
    private void ResetBox()
    {
        // 恢复玩家的移动速度为正常速度
        PM.moveSpeed = normalSpeed;

        // 解除 Box 的父物体关系
        box.SetParent(null);

        // 清除碰撞状态
        isCollidingWithPlayer = false;
    }
}
