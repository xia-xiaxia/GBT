using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public float detectionRadius ;  // 圆形检测范围的半径
    public LayerMask dreamerLayer;       // 玩家所在的层（可以通过 Inspector 设置）
    public bool isdreamerInRange = false; // 玩家是否在检测范围内
    public bool getKey;

    void Start()
    {
        getKey = false;
        gameObject.SetActive(true);
    }

    void Update()
    {
        CheckKeyInRange();
        GetKey();
    }
    void CheckKeyInRange()
    {
        // 使用 Physics2D.OverlapCircle 检测圆形范围内的物体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, dreamerLayer);

        if (colliders.Length > 0)
        {
            isdreamerInRange = true;
            getKey = true;
        }
        else
        {
            isdreamerInRange = false;
        }
    }

    void GetKey()
    {
        if (getKey)
            gameObject.SetActive(false);
    }
}
