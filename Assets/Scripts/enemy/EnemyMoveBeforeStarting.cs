using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMoveBeforeStarting: MonoBehaviour
{
    [System.Serializable]
    public class FlowPath
    {
        public string flowName; // 流程名称，便于识别
        public Transform[] waypoints; // 路径点数组
        public float waitTimeAtPoint = 0f; // 每个点的等待时间
    }

    public FlowPath[] flows; // 流程数组，在 Inspector 中配置
    public float moveSpeed = 2f; // 敌人移动速度

    private Queue<FlowPath> taskQueue = new Queue<FlowPath>(); // 任务队列
    private bool isExecuting = false;

    void Start()
    {
        // 加载流程到队列
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
            Debug.Log("所有流程完成！");
        }
    }

    private IEnumerator ExecuteFlow(FlowPath flow)
    {
        Debug.Log($"开始流程：{flow.flowName}");
        isExecuting = true;

        foreach (Transform waypoint in flow.waypoints)
        {
            yield return MoveToWaypoint(waypoint.position);
            yield return new WaitForSeconds(flow.waitTimeAtPoint); // 等待时间
        }

        isExecuting = false;
        Debug.Log($"完成流程：{flow.flowName}");
        
        StartNextTask(); // 开始下一个流程
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
