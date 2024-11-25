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
    [SerializeField] private float currentSpeed;
    public float slowSpeed;
    public float normalSpeed;
    public float quickSpeed;
    private float gridSize = 1f;
    private Vector3 targetPosition;

    private Vector3 lastPosition;  
    public bool isMoving = false;  // ��ʾ�����Ƿ������ƶ�

    private bool isCollidingWithPlayer = false; // ��ʾ�Ƿ���������ҷ�����ײ

    void Start()
    {
        box = GetComponent<Transform>();
        col = GetComponent<Collider2D>();
        currentSpeed = normalSpeed;
        lastPosition = transform.position;
    }

    void Update()
    {
        PM.moveSpeed = currentSpeed;
        DirJudge();  // �ж�����Ƿ��� Box �ƶ�
        AlignToGrid();
        if (isCollidingWithPlayer)
            isMovingorNot();
        Debug.Log(isCollidingWithPlayer);
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
        if (isCollision && collision.collider.name == "Player")
        {
            box.SetParent(collision.transform); // ���� Box Ϊ��ҵ�������
            isCollidingWithPlayer = true; // ��ǿ�ʼ��ײ����ʼ����
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
        // �������ҷ�����ײ�˳�
        if (collision.collider.name == "Player")
        {
            // �ָ���ҵ��ƶ��ٶȣ������ Box �ĸ������ϵ
            ResetBox();
        }
    }

    void AlignToGrid()
    {
        float moveSpeed = 3f;
        Vector2 currentPosition = transform.position;

        // ������ӵ����ĵ�
        float alignedX = Mathf.Floor(currentPosition.x / gridSize) * gridSize + 0.5f * gridSize;
        float alignedY = Mathf.Floor(currentPosition.y / gridSize) * gridSize + 0.5f * gridSize;

        // �ж��Ƿ��Ѿ��ڸ�������
        bool isAtGridCenter = Mathf.Abs(currentPosition.x - alignedX) < 0.1f && Mathf.Abs(currentPosition.y - alignedY) < 0.1f;

        if (isCollidingWithPlayer)
        {
            if (isAtGridCenter)
            {
                // ����Ѿ��ڸ������ģ�������ҳ���ķ��򣬼�����һ�����ӵ����ĵ�
                targetPosition = new Vector3(
                    alignedX + PM.direction.x * gridSize,  // ������ҳ�����һ������
                    alignedY + PM.direction.y * gridSize,
                    0f
                );
            }
            else
            {
                // ������ڸ������ģ�ֱ��������ǰ���ӣ�������ҳ������һ������
                targetPosition = new Vector3(
                    alignedX + PM.direction.x * gridSize,  // ������ҳ�����һ������
                    alignedY + PM.direction.y * gridSize,
                    0f
                );
            }
        }
        else
            targetPosition = new Vector3(alignedX, alignedY, 0f);

        // ʹ�� Lerp ƽ�����ɵ�Ŀ��λ��
        if (transform.position != targetPosition)
        {
            transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        }
    }

    void isMovingorNot()
    {
        // ÿ֡��������Ƿ��ƶ�
        if (transform.position != lastPosition)
        {
            isMoving = true;  // ���λ�÷����仯����Ϊ���������ƶ�
        }
        else
        {
            isMoving = false; // ���λ��û�б仯����Ϊ����û���ƶ�
        }

        // ������һ֡��λ��
        lastPosition = transform.position;
    }
    private void ResetBox()
    {
        // �ָ���ҵ��ƶ��ٶ�Ϊ�����ٶ�
        PM.moveSpeed = normalSpeed;

        // ��� Box �ĸ������ϵ
        box.SetParent(null);

        // �����ײ״̬
        isCollidingWithPlayer = false;
    }
}
