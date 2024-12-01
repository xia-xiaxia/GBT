using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFirst : MonoBehaviour
{
    public bool isWin;
    public bool isFaild;
    public EnemyFlowController enemyFlowController;
    public Transform enemyTransform;
    public float detectionRadius;  // 圆形检测范围的半径
    public LayerMask dreamerLayer;       // 玩家所在的层（可以通过 Inspector 设置）
    public bool isdreamerInRange = false; // 玩家是否在检测范围内

    void Start()
    {
        isFaild = false;
        isWin = false;
    }

    void Update()
    {
        isFaild = enemyFlowController.isGameFailed;
        StartCoroutine(nm());
        if (isWin) isFaild = false;
    }

    void CheckKeyInRange()
    {
        // 使用 Physics2D.OverlapCircle 检测圆形范围内的物体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, dreamerLayer);

        if (colliders.Length > 0)
        {
            isWin = true;

        }
        else
        {
            isFaild = true;
        }
    }

    IEnumerator nm()
    {
        yield return new WaitForSeconds(30f);
        CheckKeyInRange();
    }
}