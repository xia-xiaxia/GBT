using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possessed : MonoBehaviour
{
    public PlayerMovement PM;
    private PossessedMove PossessedMove;
    public float detectionRadius = 5f;  // 圆形检测范围的半径
    public LayerMask playerLayer;       // 玩家所在的层（可以通过 Inspector 设置）
    private bool isPlayerInRange = false; // 玩家是否在检测范围内
    public GameObject Player;

    void Start()
    {
        PossessedMove = GetComponent<PossessedMove>();
        PossessedMove.enabled = false;
    }

    void Update()
    {
        CheckPlayerInRange();
        posssessed();
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
        Gizmos.color = Color.yellow;  // 设置颜色为绿色
        Gizmos.DrawWireSphere(transform.position, detectionRadius);  // 绘制一个圆形范围
    }

    void posssessed()
    {
        if (isPlayerInRange && PM.enabled)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                PossessedMove.enabled = true;
                PM.enabled = false;
               Player.SetActive(false);
            }
        }
        if (PM.enabled == false)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                PossessedMove.enabled = false;
                PM.enabled = true;
                Player.SetActive(true);
            }
        }
    }

}
