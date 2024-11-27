using System.Collections;
using System.Collections.Generic;
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
    private float gridSize = 1f
        ;
    void Start()
    {
        box = GetComponent<Transform>();
        col = GetComponent<Collider2D>();
        currentSpeed = normalSpeed;
    }

    void Update()
    {
        PM.moveSpeed = currentSpeed;
        DirJudge();
        if (!isCollision)
            AlignToGrid();
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
        if (isCollision && collision.collider.name == "Player")
        {
            PM.moveSpeed = normalSpeed; // 恢复玩家的移动速度
            box.SetParent(null); // 解除父物体关系
        }
        if(!isCollision)
        {
            PM.moveSpeed = normalSpeed; // 恢复玩家的移动速度
            box.SetParent(null); // 解除父物体关系
        }
    }

    void AlignToGrid()
    {
        // 获取当前物体的坐标
        Vector3 currentPosition = transform.position;

        // 对齐到网格的中心点
        float alignedX = Mathf.Floor(currentPosition.x / gridSize) * gridSize + 0.5f * gridSize;
        float alignedY = Mathf.Floor(currentPosition.y / gridSize) * gridSize + 0.5f * gridSize;

        // 设置物体的位置
        transform.position = new Vector3(alignedX, alignedY, currentPosition.z);
    }
}
