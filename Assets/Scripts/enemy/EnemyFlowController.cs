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

    private Vector2 lastBoxPosition = Vector2.zero; // ��¼�ϴμ�⵽�� Box λ��
    private bool hasDetectedBox = false; // ����Ƿ��Ѿ���⵽ Box
    private bool isGameFailed = false; // ��Ϸ�Ƿ�ʧ��

    private int currentWaypointIndex = 0; // ��ǰ·��������

    private Vector2 moveDirection; // ��ǰ�ƶ�����

    void Start()
    {
        // �������̵�����
        foreach (FlowPath flow in flows)
        {
            taskQueue.Enqueue(flow);
        }
        StartNextTask();
    }

    void Update()
    {
        // �����Ϸʧ�ܣ�ֹͣһ��
        if (isGameFailed) return;

        // �ƶ�����һ��·���㲢���·���
        MoveToNextWaypoint();

        // ÿ���ƶ�������Ұ��Χ�ڵ�����
        CheckForBoxInView();

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
            Debug.Log("����������ɣ�");
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

        StartNextTask();
    }

    private IEnumerator MoveToWaypoint(Vector3 target)
    {
        // ���㵱ǰ�ƶ����򣨴ӵ�ǰλ��ָ��Ŀ��㣩
        moveDirection = (target - transform.position).normalized;

        // ���ƶ���Ŀ���
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // ȷ������Ŀ���ʱ���򱣳�һ��
        moveDirection = Vector2.zero;
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
                    // ��һ�μ��
                    if (!hasDetectedBox)
                    {
                        lastBoxPosition = boxPosition;
                        hasDetectedBox = true;
                        Debug.Log("Detected Box at position: " + lastBoxPosition);
                    }
                    else
                    {
                        // ���λ�ñ仯
                        if (lastBoxPosition != boxPosition)
                        {
                            Debug.Log("Box position changed! Game Failed!");
                            GameFailed();
                        }
                    }
                }
            }
        }
    }

    private void GameFailed()
    {
        isGameFailed = true;
        Debug.Log("Game Over!");
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
    }
}
