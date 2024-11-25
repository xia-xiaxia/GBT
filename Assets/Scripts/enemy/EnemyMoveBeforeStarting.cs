using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMoveBeforeStarting: MonoBehaviour
{
    [System.Serializable]
    public class FlowPath
    {
        public string flowName; // �������ƣ�����ʶ��
        public Transform[] waypoints; // ·��������
        public float waitTimeAtPoint = 0f; // ÿ����ĵȴ�ʱ��
    }

    public FlowPath[] flows; // �������飬�� Inspector ������
    public float moveSpeed = 2f; // �����ƶ��ٶ�

    private Queue<FlowPath> taskQueue = new Queue<FlowPath>(); // �������
    private bool isExecuting = false;

    void Start()
    {
        // �������̵�����
        foreach (FlowPath flow in flows)
        {
            taskQueue.Enqueue(flow);
        }
        StartNextTask();
    }

    private void StartNextTask()
    {
        if (taskQueue.Count > 0)
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

        foreach (Transform waypoint in flow.waypoints)
        {
            yield return MoveToWaypoint(waypoint.position);
            yield return new WaitForSeconds(flow.waitTimeAtPoint); // �ȴ�ʱ��
        }

        isExecuting = false;
        Debug.Log($"������̣�{flow.flowName}");
        
        StartNextTask(); // ��ʼ��һ������
    }

    private IEnumerator MoveToWaypoint(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
