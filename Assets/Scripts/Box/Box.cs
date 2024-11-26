using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Box : MonoBehaviour
{
    public PlayerMovement PM;
    private Transform box;
    private Collider2D col;    // Box �� Collider
    private bool isCollision = false;  // �ж��Ƿ������ײ
    private bool isMoving = false;
    [SerializeField] private float currentSpeed;
    public float slowSpeed;
    public float normalSpeed;
    public float quickSpeed;
    private float gridSize = 1f;
    private Vector3 targetPosition;
    private Vector2 boxDir;

    void Start()
    {
        box = GetComponent<Transform>();
        col = GetComponent<Collider2D>();
        currentSpeed = normalSpeed;
    }

    void Update()
    {
        PM.moveSpeed = currentSpeed;
        DirJudge();  // �ж�����Ƿ��� Box �ƶ�
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
            isCollision = true; // ��ǿ�ʼ��ײ����ʼ����
            boxDir = PM.direction;
            boxMove();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (isCollision && collision.collider.name == "Player")
        {
            currentSpeed = slowSpeed; // ����ƶ��ٶȼ���
        }
    }

    // ��ײ�˳�
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.name == "Player")
        {
            // �ָ���ҵ��ƶ��ٶȣ��������ײ״̬
            PM.moveSpeed = normalSpeed;
            isCollision = false;
            isMoving = false;
        }
    }

    void FixedUpdate()
    {
        // ��������ƶ���ʹ�ò�ֵƽ�����ɵ�Ŀ��λ��
        if (isMoving)
        {
            // ʹ�� Lerp ��ƽ�����ɵ�Ŀ��λ��
            transform.position = Vector2.Lerp(transform.position, targetPosition, currentSpeed * Time.fixedDeltaTime);

            // �ж��Ƿ񵽴�Ŀ��λ�ã������ֹͣ�ƶ�
            if (Vector2.Distance(transform.position, targetPosition) < 0.05f)
            {
                transform.position = targetPosition; // ��ȷ��Ŀ��λ��
                isMoving = false; // ֹͣ�ƶ�
            }
        }
    }


    void boxMove()
    {
        targetPosition = new Vector2(Mathf.Floor(transform.position.x / gridSize) * gridSize + 0.5f * gridSize + boxDir.x * gridSize,
                                    Mathf.Floor(transform.position.y / gridSize) * gridSize + 0.5f * gridSize + boxDir.y * gridSize);

        isMoving = true; // ���Ϊ�����ƶ�
    }

    
}
