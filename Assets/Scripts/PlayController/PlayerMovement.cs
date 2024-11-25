using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 targetPosition; // Ŀ��λ��
    public float moveSpeed; // �ƶ��ٶ�
    public float gridSize = 1f; // ÿ��Ĵ�С
    public bool isMoving = false; // �Ƿ������ƶ�
    public Vector2 direction; // ��ҵ�ǰ���ƶ�����

    private Vector3 lastPosition;
    private bool isTrans;
    private Vector3[] TargetPositions ;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = transform.position; // ��ʼĿ��λ�þ�����ҵ�ǰλ��
        TargetPositions = new Vector3[2];
    }

    void Update()
    {
        // �����������ƶ����Ͳ������µ�����
        if (isMoving)
            return;

        // ��ȡ��ҵ��ƶ�����
        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector2.up; // ����
            StartMove();
            ifBug();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            direction = Vector2.down; // ����
            StartMove();
            ifBug();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            direction = Vector2.left; // ����
            StartMove();
            ifBug();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector2.right; // ����
            StartMove();
            ifBug();
        }

    }

    void FixedUpdate()
    {
        // ��������ƶ���ʹ�ò�ֵƽ�����ɵ�Ŀ��λ��
        if (isMoving)
        {
            // ʹ�� Lerp ��ƽ�����ɵ�Ŀ��λ��
            transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);

            // �ж��Ƿ񵽴�Ŀ��λ�ã������ֹͣ�ƶ�
            if (Vector2.Distance(transform.position, targetPosition) < 0.05f)
            {
                transform.position = targetPosition; // ��ȷ��Ŀ��λ��
                isMoving = false; // ֹͣ�ƶ�
            }
        }
    }

    // �����ƶ�����
    private void StartMove()
    {
        TargetPositions[0] = targetPosition;
        // ����Ŀ��λ�ã����Ҫ����ĸ������ģ��� (n + 0.5, m + 0.5)��
        targetPosition = new Vector2(Mathf.Floor(transform.position.x / gridSize) * gridSize + 0.5f * gridSize + direction.x * gridSize,
                                     Mathf.Floor(transform.position.y / gridSize) * gridSize + 0.5f * gridSize + direction.y * gridSize);
        TargetPositions[1] = targetPosition;
        isMoving = true; // ���Ϊ�����ƶ�
    }

    void ifBug()
    {
        isMovingorNot();
        if (isMoving)
        {
            if(!isTrans)
            {
                isMoving = false;
                transform.position = TargetPositions[0];
            }
        }
    }

    void isMovingorNot()
    {
        // ÿ֡��������Ƿ��ƶ�
        if (transform.position != lastPosition)
        {
            isTrans = true;  // ���λ�÷����仯����Ϊ���������ƶ�
        }
        else
        {
            isTrans = false; // ���λ��û�б仯����Ϊ����û���ƶ�
        }

        // ������һ֡��λ��
        lastPosition = transform.position;
    }
}
