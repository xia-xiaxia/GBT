using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSecond : MonoBehaviour
{
    public bool isWin;
    public bool isFailed;
    public Transform SpyTransform;
    public EnemyFlowController enemyFlowController;
    public float detectionRadius;  // Բ�μ�ⷶΧ�İ뾶
    public LayerMask dreamerLayer;       // ������ڵĲ㣨����ͨ�� Inspector ���ã�
    public bool isdreamerInRange = false; // ����Ƿ��ڼ�ⷶΧ��
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
        // ʹ�� Physics2D.OverlapCircle ���Բ�η�Χ�ڵ�����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, dreamerLayer);

        if (colliders.Length > 0)
        {
            if (Pickupuuu.getU)
                isWin = true;
            else isWin = false;
        }
    }

}
