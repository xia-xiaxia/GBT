using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSecond : MonoBehaviour
{
    public bool isWin;
    public bool isFailed;
    public Transform SpyTransform;
    public EnemyFlowController enemyFlowController;
    public float detectionRadius;  // 圆形检测范围的半径
    public LayerMask dreamerLayer;       // 玩家所在的层（可以通过 Inspector 设置）
    public bool isdreamerInRange = false; // 玩家是否在检测范围内
    public Transform U;
    public PickupUUU Pickupuuu;

    void Start()
    {
        isWin = false;
        isFailed = false;
    }

    void Update()
    {
        isFailed = enemyFlowController.isGameFailed;
        CheckKeyInRange();
        if (isWin) isFailed = false;
    }

    void CheckKeyInRange()
    {
        // 使用 Physics2D.OverlapCircle 检测圆形范围内的物体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, dreamerLayer);

        if (colliders.Length > 0)
        {
            if (Pickupuuu.getU)
                isWin = true;
            else isWin = false;
        }
    }

}
