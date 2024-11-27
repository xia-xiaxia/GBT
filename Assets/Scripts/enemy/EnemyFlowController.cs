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
    public float hearingRange = 5f;
    private Vector2 lastBoxPosition = Vector2.zero; // 记录上次检测到的 Box 位置
    private bool hasDetectedBox = false; // 标记是否已经检测到 Box
    private bool isGameFailed = false; // 游戏是否失败
    public bool isHit=false;   
    private int currentWaypointIndex = 0; // 当前路径点索引
    public LayerMask playerLayer;   // 用于指定玩家所在的层
    public LayerMask obstacleLayer; // 用于指定障碍物所在的层
    private Vector2 moveDirection; // 当前移动方向
   private EnemyAnimationController animationController;
    // 用于存储检测到的多个 Box 的位置
    private Dictionary<int, Vector2> detectedBoxes = new Dictionary<int, Vector2>();

    void Start()
    {
       animationController = GetComponent<EnemyAnimationController>();
        // 加载流程到队列
        foreach (FlowPath flow in flows)
        {
            taskQueue.Enqueue(flow);
        }
        StartNextTask();
    }

    void Update()
    {
        // 如果游戏失败，停止敌人移动
        if (isGameFailed) return;
        //CheckForObstruction();
        CheckForPlayerInSightRange();
        // 移动到下一个路径点并更新方向
        MoveToNextWaypoint();
        float currentAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;  // 计算当前角度
        animationController.UpdateAnimation(moveDirection, currentAngle);
        CheckForBoxInView();
        CheckForWallInHearingRange();
        // 平滑转向，更新朝向，如果要做到动画切换，就不能用平滑转向了捏
      //  SmoothTurnTowardsTarget();
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
            Debug.Log("所有流程完成,，没有阻止坏结局，bekilled");
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
        GameFailed();
        StartNextTask();
    }

    private IEnumerator MoveToWaypoint(Vector3 target)
    {
        // 计算当前移动方向
        moveDirection = (target - transform.position).normalized;

        // 逐步移动到目标点,增加一个小的误差值来避免卡住
        float distanceToTarget = Vector3.Distance(transform.position, target);
        while (distanceToTarget > 0.005f) 
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            distanceToTarget = Vector3.Distance(transform.position, target);
            yield return null;
        }

        // 确保到达目标点时方向保持一致
        moveDirection = Vector2.zero;
        transform.position = target; // 确保到达目标点
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
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed);

        if (Vector3.Distance(transform.position, targetPosition) <= 0.1f) 
        {
            currentWaypointIndex++;
            //Debug.Log(currentWaypointIndex);
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
                    // 检查是否被遮挡
                    if (!IsTargetObstructed(hitCollider.transform))
                    {
                        int boxID = hitCollider.GetInstanceID(); // 获取物体的唯一 ID

                        if (!detectedBoxes.ContainsKey(boxID))
                        {
                            // 第一次检测到这个 Box
                            detectedBoxes[boxID] = boxPosition;
                            Debug.Log("第一次检测到物体的位置 " + boxPosition);
                        }
                        else
                        {
                            // 检查物体位置是否发生变化
                            if (detectedBoxes[boxID] != boxPosition)
                            {
                                Debug.Log($"箱子{boxID}被移动! Game Over.");
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
        // 获取视野范围内的所有物体
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, fieldOfViewDistance, playerLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Vector3 directionToPlayer = hitCollider.transform.position - transform.position;

                // 计算玩家与敌人之间的夹角
                float angleToPlayer = Vector3.Angle(moveDirection, directionToPlayer);

                // 如果玩家在视野角度范围内
                if (angleToPlayer < fieldOfViewAngle / 2f)
                {
                    // 检查是否被遮挡
                    if (!IsTargetObstructed(hitCollider.transform))
                    {
                        Debug.Log("玩家被发现，游戏失败！");
                        GameFailed(); // 触发游戏失败
                        return;
                    }
                }
            }
        }
    }
    private bool IsTargetObstructed(Transform targetTransform)
    {
        Vector3 directionToTarget = targetTransform.position - transform.position;

        // 使用射线检测目标是否被障碍物遮挡
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget.normalized, fieldOfViewDistance, obstacleLayer);

        // 如果射线命中，并且命中的物体不是目标本身，则目标被遮挡
        if (hit.collider != null && hit.collider.transform != targetTransform)
        {
            Debug.Log($"目标 {targetTransform.name} 被 {hit.collider.name} 遮挡，检测失败。");
            return true; // 被遮挡
        }

        return false; // 没有被遮挡
    }

   
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        // 如果敌人与障碍物发生碰撞
        if (collision.collider.CompareTag("Box"))
        {
            Debug.Log("成功阻止了bekilled的结局，游戏胜利");
            GameVictory(); // 触发游戏失败
        }
    }
    private void GameVictory()
    {
        isGameFailed = false;
        Debug.Log("阻止了坏结局，游戏胜利！");
        // 停止敌人的运动
        StopAllCoroutines();
    }
    private void CheckForWallInHearingRange()
    {
        // 检查敌人听觉范围内是否有墙壁并且敌人发生了撞击
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, hearingRange);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Wall") && isHit)
            {
                Debug.Log("听觉范围内有墙壁并发生撞击，游戏失败！");
                GameFailed();
            }
        }
    }
    private void GameFailed()
    {
        isGameFailed = true;
        Debug.Log("Game Over!");
        if (animationController != null)
        {
            animationController.SetIdleAnimation(); // 让动画转为静止状态
        }
        StopAllCoroutines();
    }
   /* private void SmoothTurnTowardsTarget()
    {
        if (currentWaypointIndex >= flows[0].waypoints.Length || isGameFailed) return;

        // 计算目标角度
        float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

        // 使用 LerpAngle 进行平滑转
        float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, turnSpeed * Time.deltaTime);

        // 应用旋转
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // 更新动画
        // 通过计算平滑后的角度来更新动画
        animationController.UpdateAnimation(moveDirection, angle);
    }

    */

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
        // 绘制听觉范围
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
