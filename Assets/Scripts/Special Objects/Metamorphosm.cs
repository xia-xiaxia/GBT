using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metamorphosm : MonoBehaviour
{
    private Vector3 temp;
    public PlayerMovement PM;
    public float detectionRadius = 5f;  // Բ�μ�ⷶΧ�İ뾶
    public LayerMask playerLayer;       // ������ڵĲ㣨����ͨ�� Inspector ���ã�
    private bool isPlayerInRange = false; // ����Ƿ��ڼ�ⷶΧ��
    private bool isMark;


    void Start()
    {
        isMark = false;
    }

    void Update()
    {
        CheckPlayerInRange();
        swap();
    }

    void CheckPlayerInRange()
    {
        // ʹ�� Physics2D.OverlapCircle ���Բ�η�Χ�ڵ�����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, playerLayer);

        if (colliders.Length > 0)
        {
            isPlayerInRange = true;
            // ���������ִ�������߼������翪ʼ׷����ҡ����ž�����Ч��
        }
        else
        {
            isPlayerInRange = false;
        }
    }

    // ���ӻ�Բ�η�Χ�����ڵ��ԣ�
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;  // ������ɫΪ��ɫ
        Gizmos.DrawWireSphere(transform.position, detectionRadius);  // ����һ��Բ�η�Χ
    }

    void swap()
    {
        if (isPlayerInRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
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
            }
        }
    }
}