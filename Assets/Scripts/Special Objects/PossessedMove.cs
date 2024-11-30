using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PossessedMove : MonoBehaviour
{
    private Vector2 targetPosition; // 目标位置
    private float moveSpeed; // 移动速度
    public float gridSize = 1f; // 每格的大小
    public bool isMoving = false; // 是否正在移动
    public Vector2 direction; // 玩家当前的移动方向
    public bool isHit;
    public PlayerMovement PM;
    public LayerMask layer;
    public float distance;
    private bool isRecovering = false;
    public bool isPush;
    public float normalSpeed;
    public float slowSpeed;

    void Start()
    {
        isPush = false;
        isHit = false;
        targetPosition = transform.position; // 初始目标位置就是玩家当前位置
        moveSpeed = normalSpeed;
    }

    void Update()
    {
        PM.isPossessed = true;
        PM.isWalk = isMoving;
        PM.direction = direction;
        if (isMoving) return;
        // 获取玩家的移动输入
        if (isPush)
            moveSpeed = slowSpeed;
        else
            moveSpeed = normalSpeed;
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
            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition; // 精确到目标位置
                isMoving = false; // 停止移动
            }
        }
    }

    private void StartMove()
    {
        if (isRecovering) return;
        examHinder();
        if (isHit) return;
        if(isPush) 
        // 计算目标位置（玩家要到达的格子中心，即 (n + 0.5, m + 0.5)）
        targetPosition = new Vector2(Mathf.Floor(transform.position.x / gridSize) * gridSize + 0.5f * gridSize + direction.x * gridSize + direction.x * 0.05f,
                                    Mathf.Floor(transform.position.y / gridSize) * gridSize + 0.5f * gridSize + direction.y * gridSize + direction.y * 0.05f);
        else
            targetPosition = new Vector2(Mathf.Floor(transform.position.x / gridSize) * gridSize + 0.5f * gridSize + direction.x * gridSize,
                                    Mathf.Floor(transform.position.y / gridSize) * gridSize + 0.5f * gridSize + direction.y * gridSize);
        isMoving = true; // 标记为正在移动
    }

    private void examHinder()
    {
        Vector3 origin = transform.position;
        Vector3 dir = direction; // direction 已经是世界坐标方向
        int layerMask = 1 << LayerMask.NameToLayer("Hinder");
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, distance, layerMask);


        if (hit.collider != null)  // 如果射线碰到了物体
        {
            // 输出碰撞物体的信息
            Debug.Log("碰撞到的物体: " + hit.collider.name);
            Debug.DrawLine(origin, hit.point, Color.red);  // 可视化射线
            isHit = true;
            PM.isMoving = false;
            isMoving = false;
            StartCoroutine(recovery());
        }
        else
        {
            isHit = false;
            // 如果没有碰撞，可视化射线
            Debug.DrawRay(origin, direction * distance, Color.white);
        }
    }
    IEnumerator recovery()
    {
        isRecovering = true;
        yield return new WaitForSeconds(0.2f);
        isHit = false;
        isRecovering = false;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    int layer = collision.collider.gameObject.layer;
    //    if (layer == LayerMask.NameToLayer("Hinder"))
    //    {
    //        isMoving = false;
    //        isHit = true;
    //        calculate();
    //        transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
    //    }
    //}
    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    int layer = collision.collider.gameObject.layer;
    //    if (layer == LayerMask.NameToLayer("Hinder"))
    //    {
    //        calculate();
    //        transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    int layer = collision.collider.gameObject.layer;
    //    if (layer == LayerMask.NameToLayer("Hinder"))
    //    {
    //        isHit = false;
    //        calculate();
    //        transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
    //    }
    //}

    //private void calculate()
    //{
    //    targetPosition = new Vector2(Mathf.Floor(transform.position.x / gridSize) * gridSize + 0.5f * gridSize,
    //                                 Mathf.Floor(transform.position.y / gridSize) * gridSize + 0.5f * gridSize);
    //}

}
