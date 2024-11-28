using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessedMove : MonoBehaviour
{
    private Vector2 targetPosition; // Ŀ��λ��
    public float moveSpeed = 4f; // �ƶ��ٶ�
    public float gridSize = 1f; // ÿ��Ĵ�С
    public bool isMoving = false; // �Ƿ������ƶ�
    public Vector2 direction; // ��ҵ�ǰ���ƶ�����
    public bool isHit = false;
    public PlayerMovement PM;
    public LayerMask layer;

    void Start()
    {
        targetPosition = transform.position; // ��ʼĿ��λ�þ�����ҵ�ǰλ��
    }

    void Update()
    {
        // ��������ƶ����ͺ�������
        if (PM != null)
        {
            if (PM.isMoving) return;
        }
        else
        {
            if (isMoving)
                return;
        }

        // ��ȡ��ҵ��ƶ�����
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
            if (Vector2.Distance(transform.position, targetPosition) < 0.05f)
            {
                transform.position = targetPosition; // ��ȷ��Ŀ��λ��
                isMoving = false; // ֹͣ�ƶ�
            }
        }
    }

    private void StartMove()
    {
        // ����Ŀ��λ�ã����Ҫ����ĸ������ģ��� (n + 0.5, m + 0.5)��
        targetPosition = new Vector2(Mathf.Floor(transform.position.x / gridSize) * gridSize + 0.5f * gridSize + direction.x * gridSize,
                                     Mathf.Floor(transform.position.y / gridSize) * gridSize + 0.5f * gridSize + direction.y * gridSize);
        isMoving = true; // ���Ϊ�����ƶ�
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        int layer = collision.collider.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Hinder"))
        {
            isMoving = false;
            isHit = true;
            calculate();
            transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        int layer = collision.collider.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Hinder"))
        {
            calculate();
            transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        int layer = collision.collider.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Hinder"))
        {
            isHit = false;
            calculate();
            transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void calculate()
    {
        targetPosition = new Vector2(Mathf.Floor(transform.position.x / gridSize) * gridSize + 0.5f * gridSize,
                                     Mathf.Floor(transform.position.y / gridSize) * gridSize + 0.5f * gridSize);
    }
    IEnumerator recovery()
    {
        yield return new WaitForSeconds(0.2f);
        isHit = false;
    }
}
