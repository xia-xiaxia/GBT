using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PossessedMove : MonoBehaviour
{
    private Vector2 targetPosition; // Ŀ��λ��
    private float moveSpeed; // �ƶ��ٶ�
    public float gridSize = 1f; // ÿ��Ĵ�С
    public bool isMoving = false; // �Ƿ������ƶ�
    public Vector2 direction; // ��ҵ�ǰ���ƶ�����
    public bool isHit;
    public PlayerMovement PM;
    public LayerMask layer;
    public float distance;
    private bool isRecovering = false;
    public bool isPush;
    public float normalSpeed;
    public float slowSpeed;

    void Start()
    {
        isPush = false;
        isHit = false;
        targetPosition = transform.position; // ��ʼĿ��λ�þ�����ҵ�ǰλ��
        moveSpeed = normalSpeed;
    }

    void Update()
    {
        PM.isPossessed = true;
        PM.isWalk = isMoving;
        PM.direction = direction;
        if (isMoving) return;
        // ��ȡ��ҵ��ƶ�����
        if (isPush)
            moveSpeed = slowSpeed;
        else
            moveSpeed = normalSpeed;
        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector2.up; // ����
            StartMove();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            direction = Vector2.down; // ����
            StartMove();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            direction = Vector2.left; // ����
            StartMove();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector2.right; // ����
            StartMove();
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
            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition; // ��ȷ��Ŀ��λ��
                isMoving = false; // ֹͣ�ƶ�
            }
        }
    }

    private void StartMove()
    {
        if (isRecovering) return;
        examHinder();
        if (isHit) return;
        if(isPush) 
        // ����Ŀ��λ�ã����Ҫ����ĸ������ģ��� (n + 0.5, m + 0.5)��
        targetPosition = new Vector2(Mathf.Floor(transform.position.x / gridSize) * gridSize + 0.5f * gridSize + direction.x * gridSize + direction.x * 0.05f,
                                    Mathf.Floor(transform.position.y / gridSize) * gridSize + 0.5f * gridSize + direction.y * gridSize + direction.y * 0.05f);
        else
            targetPosition = new Vector2(Mathf.Floor(transform.position.x / gridSize) * gridSize + 0.5f * gridSize + direction.x * gridSize,
                                    Mathf.Floor(transform.position.y / gridSize) * gridSize + 0.5f * gridSize + direction.y * gridSize);
        isMoving = true; // ���Ϊ�����ƶ�
    }

    private void examHinder()
    {
        Vector3 origin = transform.position;
        Vector3 dir = direction; // direction �Ѿ����������귽��
        int layerMask = 1 << LayerMask.NameToLayer("Hinder");
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, distance, layerMask);


        if (hit.collider != null)  // �����������������
        {
            // �����ײ�������Ϣ
            Debug.Log("��ײ��������: " + hit.collider.name);
            Debug.DrawLine(origin, hit.point, Color.red);  // ���ӻ�����
            isHit = true;
            PM.isMoving = false;
            isMoving = false;
            StartCoroutine(recovery());
        }
        else
        {
            isHit = false;
            // ���û����ײ�����ӻ�����
            Debug.DrawRay(origin, direction * distance, Color.white);
        }
    }
    IEnumerator recovery()
    {
        isRecovering = true;
        yield return new WaitForSeconds(0.2f);
        isHit = false;
        isRecovering = false;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    int layer = collision.collider.gameObject.layer;
    //    if (layer == LayerMask.NameToLayer("Hinder"))
    //    {
    //        isMoving = false;
    //        isHit = true;
    //        calculate();
    //        transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
    //    }
    //}
    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    int layer = collision.collider.gameObject.layer;
    //    if (layer == LayerMask.NameToLayer("Hinder"))
    //    {
    //        calculate();
    //        transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    int layer = collision.collider.gameObject.layer;
    //    if (layer == LayerMask.NameToLayer("Hinder"))
    //    {
    //        isHit = false;
    //        calculate();
    //        transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
    //    }
    //}

    //private void calculate()
    //{
    //    targetPosition = new Vector2(Mathf.Floor(transform.position.x / gridSize) * gridSize + 0.5f * gridSize,
    //                                 Mathf.Floor(transform.position.y / gridSize) * gridSize + 0.5f * gridSize);
    //}

}
