using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyFlowController : MonoBehaviour
{
    [System.Serializable]
    public class FlowPath
    {
        public string flowName; // 流程名称，便于识别
        public Transform[] waypoints; // 路径点数组
        public float waitTimeAtPoint = 0f; // 每个点的等待时间
    }

    public FlowPath[] flows; // 流程数组，在 Inspector 中配置
    public float moveSpeed = 0.5f; // 敌人移动速度
    public float turnSpeed = 5f; // 转向平滑速度

    private Queue<FlowPath> taskQueue = new Queue<FlowPath>(); // 任务队列
    private bool isExecuting = false;

    public float fieldOfViewDistance = 5f; // 视野范围
    public float fieldOfViewAngle = 110f; // 视野角度

    private Vector2 lastBoxPosition = Vector2.zero; // 记录上次检测到的 Box 位置
    private bool hasDetectedBox = false; // 标记是否已经检测到 Box
    private bool isGameFailed = false; // 游戏是否失败

    private int currentWaypointIndex = 0; // 当前路径点索引

    private Vector2 moveDirection; // 当前移动方向

    void Start()
    {
        // 加载流程到队列
        foreach (FlowPath flow in flows)
        {
            taskQueue.Enqueue(flow);
        }
        StartNextTask();
    }

    void Update()
    {
        // 如果游戏失败，停止一切
        if (isGameFailed) return;

        // 移动到下一个路径点并更新方向
        MoveToNextWaypoint();

        // 每次移动后检查视野范围内的物体
        CheckForBoxInView();

        // 平滑转向，更新朝向
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
            Debug.Log("所有流程完成！");
        }
    }

    private IEnumerator ExecuteFlow(FlowPath flow)
    {
        Debug.Log($"开始流程：{flow.flowName}");
        isExecuting = true;

        // 初始化路径点索引
        currentWaypointIndex = 0;

        while (currentWaypointIndex < flow.waypoints.Length)
        {
            Transform waypoint = flow.waypoints[currentWaypointIndex];

            // 移动到当前路径点
            yield return MoveToWaypoint(waypoint.position);

            // 等待指定时间
            yield return new WaitForSeconds(flow.waitTimeAtPoint);

            // 更新到下一个路径点
            currentWaypointIndex++;
        }

        isExecuting = false;
        Debug.Log($"完成流程：{flow.flowName}");

        StartNextTask();
    }

    private IEnumerator MoveToWaypoint(Vector3 target)
    {
        // 计算当前移动方向（从当前位置指向目标点）
        moveDirection = (target - transform.position).normalized;

        // 逐步移动到目标点
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // 确保到达目标点时方向保持一致
        moveDirection = Vector2.zero;
    }

    private void MoveToNextWaypoint()
    {
        if (isGameFailed || currentWaypointIndex >= flows[0].waypoints.Length) return;

        // 获取当前目标路径点
        Transform targetWaypoint = flows[0].waypoints[currentWaypointIndex];
        Vector3 targetPosition = targetWaypoint.position;

        // 计算移动方向
        moveDirection = (targetPosition - transform.position).normalized;

        // 移动敌人
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 如果到达路径点，更新索引
        if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
        {
            currentWaypointIndex++;
        }
    }

    private void CheckForBoxInView()
    {
        if (isGameFailed) return;

        // 获取视野范围内的物体
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, fieldOfViewDistance);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Box"))
            {
                Vector2 boxPosition = hitCollider.transform.position;
                Vector2 toBox = (boxPosition - (Vector2)transform.position).normalized;

                // 检查是否在视野角度内
                float angleToBox = Vector2.Angle(moveDirection, toBox);

                if (angleToBox < fieldOfViewAngle / 2f)
                {
                    // 第一次检测
                    if (!hasDetectedBox)
                    {
                        lastBoxPosition = boxPosition;
                        hasDetectedBox = true;
                        Debug.Log("Detected Box at position: " + lastBoxPosition);
                    }
                    else
                    {
                        // 检查位置变化
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

        // 平滑转向的目标角度
        float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

        // 平滑旋转
        float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, turnSpeed * Time.deltaTime);

        // 应用旋转
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnDrawGizmos()
    {
        if (isGameFailed) return;

        Gizmos.color = Color.red;

        // 绘制视野扇形
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
