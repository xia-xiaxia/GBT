using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyFlowController : MonoBehaviour
{
    [System.Serializable]
    public class FlowPath
    {
        public string flowName; // �������ƣ�����ʶ��
        public Transform[] waypoints; // ·��������
        public float waitTimeAtPoint = 0f; // ÿ����ĵȴ�ʱ��
    }

    public FlowPath[] flows; // �������飬�� Inspector ������
    public float moveSpeed = 0.5f; // �����ƶ��ٶ�
    public float turnSpeed = 5f; // ת��ƽ���ٶ�

    private Queue<FlowPath> taskQueue = new Queue<FlowPath>(); // �������
    private bool isExecuting = false;

    public float fieldOfViewDistance = 5f; // ��Ұ��Χ
    public float fieldOfViewAngle = 110f; // ��Ұ�Ƕ�
    public float hearingRange = 5f;
    private Vector2 lastBoxPosition = Vector2.zero; // ��¼�ϴμ�⵽�� Box λ��
    private bool hasDetectedBox = false; // ����Ƿ��Ѿ���⵽ Box
    private bool isGameFailed = false; // ��Ϸ�Ƿ�ʧ��
    public bool isHit=false;   
    private int currentWaypointIndex = 0; // ��ǰ·��������
    public LayerMask playerLayer;   // ����ָ��������ڵĲ�
    public LayerMask obstacleLayer; // ����ָ���ϰ������ڵĲ�
    private Vector2 moveDirection; // ��ǰ�ƶ�����
   // private EnemyAnimationController animationController;
    // ���ڴ洢��⵽�Ķ�� Box ��λ��
    private Dictionary<int, Vector2> detectedBoxes = new Dictionary<int, Vector2>();

    void Start()
    {
       // animationController = GetComponent<EnemyAnimationController>();
        // �������̵�����
        foreach (FlowPath flow in flows)
        {
            taskQueue.Enqueue(flow);
        }
        StartNextTask();
    }

    void Update()
    {
        // �����Ϸʧ�ܣ�ֹͣ�����ƶ�
        if (isGameFailed) return;
        //CheckForObstruction();
        CheckForPlayerInSightRange();
        // �ƶ�����һ��·���㲢���·���
        MoveToNextWaypoint();
        //animationController.UpdateAnimation(moveDirection);
        // ÿ���ƶ�������Ұ��Χ�ڵ�����
        CheckForBoxInView();
        CheckForWallInHearingRange();
        // ƽ��ת�򣬸��³���
        SmoothTurnTowardsTarget();
    }

    private void StartNextTask()
    {
        if (taskQueue.Count > 0 && !isGameFailed)
        {
            FlowPath currentFlow = taskQueue.Dequeue();
            StartCoroutine(ExecuteFlow(currentFlow));
        }
        else
        {
            Debug.Log("�����������,��û����ֹ����֣�bekilled");
        }
    }

    private IEnumerator ExecuteFlow(FlowPath flow)
    {
        Debug.Log($"��ʼ���̣�{flow.flowName}");
        isExecuting = true;

        // ��ʼ��·��������
        currentWaypointIndex = 0;

        while (currentWaypointIndex < flow.waypoints.Length)
        {
            Transform waypoint = flow.waypoints[currentWaypointIndex];

            // �ƶ�����ǰ·����
            yield return MoveToWaypoint(waypoint.position);

            // �ȴ�ָ��ʱ��
            yield return new WaitForSeconds(flow.waitTimeAtPoint);

            // ���µ���һ��·����
            currentWaypointIndex++;
        }

        isExecuting = false;
        Debug.Log($"������̣�{flow.flowName}");
        GameFailed();
        StartNextTask();
    }

    private IEnumerator MoveToWaypoint(Vector3 target)
    {
        // ���㵱ǰ�ƶ�����
        moveDirection = (target - transform.position).normalized;

        // ���ƶ���Ŀ���
        while (Vector3.Distance(transform.position, target) > 0.005f) // ʹ�ø�С�����
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // ȷ������Ŀ���ʱ���򱣳�һ��
        moveDirection = Vector2.zero;
        transform.position = target; // ȷ������Ŀ���
    }

    private void MoveToNextWaypoint()
    {
        if (isGameFailed || currentWaypointIndex >= flows[0].waypoints.Length) return;

        // ��ȡ��ǰĿ��·����
        Transform targetWaypoint = flows[0].waypoints[currentWaypointIndex];
        Vector3 targetPosition = targetWaypoint.position;

        // �����ƶ�����
        moveDirection = (targetPosition - transform.position).normalized;

        // �ƶ�����
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        // �������·���㣬��������
        if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
        {
            currentWaypointIndex++;
        }
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
                        int boxID = hitCollider.GetInstanceID(); // ��ȡ�����Ψһ ID

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

        // ����������У��������е����岻��Ŀ�걾������Ŀ�걻�ڵ�
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
            GameVictory(); // ������Ϸʧ��
        }
    }
    private void GameVictory()
    {
        isGameFailed = false;
        Debug.Log("��ֹ�˻���֣���Ϸʤ����");
        // ֹͣ���˵��˶�
        StopAllCoroutines();
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
                GameFailed();
            }
        }
    }
    private void GameFailed()
    {
        isGameFailed = true;
        Debug.Log("Game Over!");
        StopAllCoroutines();
    }

    private void SmoothTurnTowardsTarget()
    {
        if (currentWaypointIndex >= flows[0].waypoints.Length || isGameFailed) return;

        // ƽ��ת���Ŀ��Ƕ�
        float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

        // ƽ����ת
        float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, turnSpeed * Time.deltaTime);

        // Ӧ����ת
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
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

        //if (Application.isPlaying)
        //{
          //  foreach (var box in detectedBoxes.Values)
           // {
          //      Gizmos.color = Color.yellow;
            //    Gizmos.DrawLine(transform.position, box);
            //}
       // }
    }
}