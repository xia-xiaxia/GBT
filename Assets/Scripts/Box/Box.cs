using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Box : MonoBehaviour
{
    public PossessedMove PM;
    private Transform box;
    private Collider2D col;    // Box 的 Collider
    private bool isCollision = false;  // 判断是否可以碰撞
    private bool isMoving = false;
    private float gridSize = 1f;
    public Vector3 targetPosition;
    private Vector2 boxDir;
    private bool canPush;
    public Wall wallconl;
    public float distance;


    void Start()
    {
        targetPosition = transform.position;
        box = GetComponent<Transform>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        DirJudge();  // 判断玩家是否朝向 Box 移动
        canPush = wallconl.canTrans;
        isWall();
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
            PM.isPush = true;
            isCollision = true; // 标记开始碰撞，开始对齐
            boxDir = PM.direction;
            boxMove();
        }

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (isCollision && collision.collider.name == "Player")
        {
            PM.isPush = true;
        }
    }

    // 碰撞退出
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.name == "Player")
        {
            PM.isPush = false;
            // 清除碰撞状态
            isCollision = false;
            isMoving = false;
            boxDir = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        // 如果正在移动，使用插值平滑过渡到目标位置
        if (isMoving)
        {
            // 使用 Lerp 来平滑过渡到目标位置
            transform.position = Vector2.Lerp(transform.position, targetPosition, PM.slowSpeed * Time.fixedDeltaTime);

            // 判断是否到达目标位置，若到达，停止移动
            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition; // 精确到目标位置
                isMoving = false; // 停止移动
            }
        }
    }


    void boxMove()
    {
        targetPosition = new Vector2(Mathf.Floor(transform.position.x / gridSize) * gridSize + 0.5f * gridSize + boxDir.x * gridSize + boxDir.x * 0.12f,
                                    Mathf.Floor(transform.position.y / gridSize) * gridSize + 0.5f * gridSize + boxDir.y * gridSize + boxDir.y * 0.12f);

        isMoving = true; // 标记为正在移动
    }

    void isWall()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.TransformDirection(boxDir);
        int layerMask = 1 << LayerMask.NameToLayer("Hinder");
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layerMask);


        if (hit.collider != null)  // 如果射线碰到了物体
        {
            // 输出碰撞物体的信息
            Debug.Log("碰撞到的物体: " + hit.collider.name);
            Debug.DrawLine(origin, hit.point, Color.yellow, 1f);  // 可视化射线
            PM.isHit = true;
            PM.isMoving = false;
            isMoving = false;
            StartCoroutine(recovery());
        }
        else
        {
            PM.isHit = false;
            // 如果没有碰撞，可视化射线
            Debug.DrawRay(origin, direction * distance, Color.white, 1f);
        }
    }

    IEnumerator recovery()
    {
        yield return new WaitForSeconds(1f);
        boxDir = Vector3.zero;
        PM.isHit = false;
    }

}