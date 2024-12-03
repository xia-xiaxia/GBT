using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sky1controller : MonoBehaviour
{
    public Transform[] waypoints; // ·��������
    public float moveSpeed = 2f; // �����ƶ��ٶ�
    private Transform targetWaypoint;
    public bool isExecuting = false;
    public float fieldOfViewDistance = 5f; // ��Ұ��Χ
    public float fieldOfViewAngle = 110f; // ��Ұ�Ƕ�
    public float hearingRange = 5f;
    private Vector2 lastBoxPosition = Vector2.zero; // ��¼�ϴμ�⵽�� Box λ��
    private bool hasDetectedBox = false; // ����Ƿ��Ѿ���⵽ Box
    public bool isGameFailed = false; // ��Ϸ�Ƿ�ʧ��
    public bool isHit = false;   // �Ƿ�ײ��ǽ�ڷ�������
    private int currentWaypointIndex = 0; // ��ǰ·��������
    public LayerMask playerLayer;   // ������ڵĲ�
    public LayerMask obstacleLayer; // �ϰ���ɽ��������ڵĲ�
    private Vector2 moveDirection; // ��ǰ�ƶ�����                        
    private SpyAnimationController animationController;
    // �洢��� Box ��λ��
    private Dictionary<int, Vector2> detectedBoxes = new Dictionary<int, Vector2>();
    void Start()
    {
        animationController = GetComponent<SpyAnimationController>();
        
            if (waypoints.Length > 0)
            {
                targetWaypoint = waypoints[currentWaypointIndex];
            }
   
    }

    void Update()
    {
        if (isGameFailed)
            return;
      
        if (waypoints.Length == 0) return;

        // �ƶ���Ŀ�꺽��
        Vector2 direction = targetWaypoint.position - transform.position;
        moveDirection = direction.normalized; 

        // ���㵱ǰ�Ƕ�
        float currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);
        //�Ƿ񵽴�
        if ((Vector2)transform.position == (Vector2)targetWaypoint.position)
        {
            currentWaypointIndex++;
            Debug.Log(currentWaypointIndex);

            //�������һ�����ʱ��ֹͣ�ƶ�
            if (currentWaypointIndex >= waypoints.Length)
            {
                enabled = false;
                animationController.SetIdleAnimation();
                return;
            }

            // �����ƶ�����һ��·����
            targetWaypoint = waypoints[currentWaypointIndex];
        }

        // ���ݽǶ��ж϶������߼�
        animationController.UpdateAnimation(moveDirection, currentAngle);
        CheckForBoxInView();
        CheckForPlayerInSightRange();
        CheckForWallInHearingRange();
    }

    private void CheckForBoxInView()
    {
        if (isGameFailed) return;

        // ��ȡ��Ұ��Χ�ڵ�����
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, fieldOfViewDistance);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Box"))
            {
                Vector2 boxPosition = hitCollider.transform.position;
                Vector2 toBox = (boxPosition - (Vector2)transform.position).normalized;

                // ����Ƿ�����Ұ�Ƕ���
                float angleToBox = Vector2.Angle(moveDirection, toBox);

                if (angleToBox < fieldOfViewAngle / 2f)
                {
                    // ����Ƿ��ڵ�
                    if (!IsTargetObstructed(hitCollider.transform))
                    {
                        int boxID = hitCollider.GetInstanceID(); //��ȡid

                        if (!detectedBoxes.ContainsKey(boxID))
                        {
                            // ��һ�μ�⵽��� Box
                            detectedBoxes[boxID] = boxPosition;
                            Debug.Log("��һ�μ�⵽�����λ�� " + boxPosition);
                        }
                        else
                        {
                            // �������λ���Ƿ����仯
                            if (detectedBoxes[boxID] != boxPosition)
                            {
                                Debug.Log($"����{boxID}���ƶ�! Game Over.");
                                GameFailed();
                            }
                        }
                    }
                }
            }
        }
    }
    private void CheckForPlayerInSightRange()
    {
        // ��ȡ��Ұ��Χ�ڵ���������
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, fieldOfViewDistance, playerLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Vector3 directionToPlayer = hitCollider.transform.position - transform.position;

                // ������������֮��ļн�
                float angleToPlayer = Vector3.Angle(moveDirection, directionToPlayer);

                // ����������Ұ�Ƕȷ�Χ��
                if (angleToPlayer < fieldOfViewAngle / 2f)
                {
                    // ����Ƿ��ڵ�
                    if (!IsTargetObstructed(hitCollider.transform))
                    {
                        Debug.Log("��ұ����֣���Ϸʧ�ܣ�");
                        animationController.SetIdleAnimation();
                        GameFailed(); // ������Ϸʧ��
                        return;
                    }
                }
            }
        }
    }
    private bool IsTargetObstructed(Transform targetTransform)
    {
        Vector3 directionToTarget = targetTransform.position - transform.position;

        // ʹ�����߼��Ŀ���Ƿ��ϰ����ڵ�
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget.normalized, fieldOfViewDistance, obstacleLayer);

        // ����������У��������е����岻��Ŀ�걾����Ŀ�걻�ڵ�
        if (hit.collider != null && hit.collider.transform != targetTransform)
        {
            Debug.Log($"Ŀ�� {targetTransform.name} �� {hit.collider.name} �ڵ������ʧ�ܡ�");
            return true; // ���ڵ�
        }

        return false; // û�б��ڵ�
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ����������ϰ��﷢����ײ
        if (collision.collider.CompareTag("Box"))
        {
            Debug.Log("�ɹ���ֹ��bekilled�Ľ�֣���Ϸʤ��");
            animationController.SetIdleAnimation();
            GameVictory(); // ��Ϸʤ��
        }
    } 
     private void GameVictory()
    {
        isGameFailed = false;
        Debug.Log("��ֹ�˻���֣���Ϸʤ����");
        animationController.SetIdleAnimation();
      
    
    }
    private void CheckForWallInHearingRange()
    {
        // ������������Χ���Ƿ���ǽ�ڲ��ҵ��˷�����ײ��
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, hearingRange);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Wall") && isHit)
            {
                Debug.Log("������Χ����ǽ�ڲ�����ײ������Ϸʧ�ܣ�");
                animationController.SetIdleAnimation();
                GameFailed();
            }
        }
    }
    private void GameFailed()
    {
        isGameFailed = true;
        Debug.Log("Game Over!");
        animationController.SetIdleAnimation();
     
    }
    private void OnDrawGizmos()
    {
        if (isGameFailed) return;

        Gizmos.color = Color.red;

        // ������Ұ����
        Vector3 origin = transform.position;
        Vector3 forward = moveDirection;

        float step = 1f;
        for (float angle = -fieldOfViewAngle / 2f; angle <= fieldOfViewAngle / 2f; angle += step)
        {
            Vector3 direction = Quaternion.Euler(0, 0, angle) * forward;
            Gizmos.DrawLine(origin, origin + direction * fieldOfViewDistance);
        }
        // ����������Χ
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hearingRange);

        if (Application.isPlaying)
        {
            foreach (var box in detectedBoxes.Values)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, box);
            }
        }
    }
}