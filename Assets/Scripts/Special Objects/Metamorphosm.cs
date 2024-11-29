using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metamorphosm : MonoBehaviour
{
    private Vector3 temp;
    public PlayerMovement PM;
    public float detectionRadius = 5f;  // 圆形检测范围的半径
    public LayerMask playerLayer;       // 玩家所在的层（可以通过 Inspector 设置）
    public bool isPlayerInRange = false; // 玩家是否在检测范围内
    public bool isMark;
    private int count;
    public int maxCount;
    public int n;


    void Start()
    {
        isMark = false;
        count = 0;
    }

    void Update()
    {
        if (!PM.isPossessed)
        {
            CheckPlayerInRange();
            if (count < maxCount)
            {
                if (!PM.isMoving)
                    swap();
            }
        }
    }

    void CheckPlayerInRange()
    {
        // 使用 Physics2D.OverlapCircle 检测圆形范围内的物体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, playerLayer);

        if (colliders.Length > 0)
        {
            isPlayerInRange = true;
            // 在这里可以执行其他逻辑，例如开始追踪玩家、播放警告音效等
        }
        else
        {
            isPlayerInRange = false;
        }
    }

    // 可视化圆形范围（用于调试）
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;  // 设置颜色为绿色
        Gizmos.DrawWireSphere(transform.position, detectionRadius);  // 绘制一个圆形范围
    }

    void swap()
    {
        if (isPlayerInRange)
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                isMark = true;
            }
        }

            if (isMark)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    temp = PM.transform.position;
                    PM.transform.position = transform.position;
                    transform.position = temp;
                    isMark = false;
                    count++;
                }
            }
    }

}
