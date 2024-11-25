using System.Collections;
using System.Collections.Generic;
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
    private float gridSize = 1f
        ;
    void Start()
    {
        box = GetComponent<Transform>();
        col = GetComponent<Collider2D>();
        currentSpeed = normalSpeed;
    }

    void Update()
    {
        PM.moveSpeed = currentSpeed;
        DirJudge();
        if (!isCollision)
            AlignToGrid();
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
        if (isCollision && collision.collider.name == "Player")
        {
            PM.moveSpeed = normalSpeed; // �ָ���ҵ��ƶ��ٶ�
            box.SetParent(null); // ����������ϵ
        }
        if(!isCollision)
        {
            PM.moveSpeed = normalSpeed; // �ָ���ҵ��ƶ��ٶ�
            box.SetParent(null); // ����������ϵ
        }
    }

    void AlignToGrid()
    {
        // ��ȡ��ǰ���������
        Vector3 currentPosition = transform.position;

        // ���뵽��������ĵ�
        float alignedX = Mathf.Floor(currentPosition.x / gridSize) * gridSize + 0.5f * gridSize;
        float alignedY = Mathf.Floor(currentPosition.y / gridSize) * gridSize + 0.5f * gridSize;

        // ���������λ��
        transform.position = new Vector3(alignedX, alignedY, currentPosition.z);
    }
}
