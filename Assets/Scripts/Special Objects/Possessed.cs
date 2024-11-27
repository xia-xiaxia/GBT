using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possessed : MonoBehaviour
{
    public PlayerMovement PM;
    private PossessedMove PossessedMove;
    public float detectionRadius = 5f;  // Բ�μ�ⷶΧ�İ뾶
    public LayerMask playerLayer;       // ������ڵĲ㣨����ͨ�� Inspector ���ã�
    private bool isPlayerInRange = false; // ����Ƿ��ڼ�ⷶΧ��
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
        Gizmos.color = Color.yellow;  // ������ɫΪ��ɫ
        Gizmos.DrawWireSphere(transform.position, detectionRadius);  // ����һ��Բ�η�Χ
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
