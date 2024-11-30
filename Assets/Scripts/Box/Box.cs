using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Box : MonoBehaviour
{
    public PossessedMove PM;
    private Transform box;
    private Collider2D col;    // Box �� Collider
    private bool isCollision = false;  // �ж��Ƿ������ײ
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
        DirJudge();  // �ж�����Ƿ��� Box �ƶ�
        canPush = wallconl.canTrans;
        isWall();
    }

    // �ж�����Ƿ��� Box �ƶ�
    void DirJudge()
    {
        Vector3 dirToBox = box.position - PM.transform.position;
        float angle = Vector3.Angle(PM.direction, dirToBox.normalized);

        if (angle < 45f)
        {
            isCollision = true; // �����ƶ�
        }
        else
        {
            isCollision = false; // �������ƶ�
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Player")
        {
            PM.isPush = true;
            isCollision = true; // ��ǿ�ʼ��ײ����ʼ����
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

    // ��ײ�˳�
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.name == "Player")
        {
            PM.isPush = false;
            // �����ײ״̬
            isCollision = false;
            isMoving = false;
            boxDir = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        // ��������ƶ���ʹ�ò�ֵƽ�����ɵ�Ŀ��λ��
        if (isMoving)
        {
            // ʹ�� Lerp ��ƽ�����ɵ�Ŀ��λ��
            transform.position = Vector2.Lerp(transform.position, targetPosition, PM.slowSpeed * Time.fixedDeltaTime);

            // �ж��Ƿ񵽴�Ŀ��λ�ã������ֹͣ�ƶ�
            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition; // ��ȷ��Ŀ��λ��
                isMoving = false; // ֹͣ�ƶ�
            }
        }
    }


    void boxMove()
    {
        targetPosition = new Vector2(Mathf.Floor(transform.position.x / gridSize) * gridSize + 0.5f * gridSize + boxDir.x * gridSize + boxDir.x * 0.12f,
                                    Mathf.Floor(transform.position.y / gridSize) * gridSize + 0.5f * gridSize + boxDir.y * gridSize + boxDir.y * 0.12f);

        isMoving = true; // ���Ϊ�����ƶ�
    }

    void isWall()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.TransformDirection(boxDir);
        int layerMask = 1 << LayerMask.NameToLayer("Hinder");
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layerMask);


        if (hit.collider != null)  // �����������������
        {
            // �����ײ�������Ϣ
            Debug.Log("��ײ��������: " + hit.collider.name);
            Debug.DrawLine(origin, hit.point, Color.yellow, 1f);  // ���ӻ�����
            PM.isHit = true;
            PM.isMoving = false;
            isMoving = false;
            StartCoroutine(recovery());
        }
        else
        {
            PM.isHit = false;
            // ���û����ײ�����ӻ�����
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