using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    [System.Serializable]
    public class FlowPath
    {
        public string flowName; // 流程名称
        public Transform[] waypoints; // 路径点数组
        public float waitTimeAtPoint = 0f; // 每个点的等待时间
    }

    public FlowPath[] flows; // 流程数组
    public float moveSpeed = 0.5f; // 敌人移动速度
    private Queue<FlowPath> taskQueue = new Queue<FlowPath>(); // 任务队列
    public bool isExecuting = false;
    public float fieldOfViewDistance = 5f; // 视野范围
    public float fieldOfViewAngle = 110f;
    public float hearingRange = 5f;// 视野角度
    private Vector2 lastBoxPosition = Vector2.zero; // 记录上次检测到的 Box 位置
    private bool hasDetectedBox = false; // 标记是否已经检测到 Box
    private bool isGameFailed = false;
    public bool isHit = false;// 游戏是否失败
    private int currentWaypointIndex = 0; // 当前路径点索引
    public LayerMask playerLayer;   // 玩家所在的层
    public LayerMask obstacleLayer;
    public LayerMask folderLayer; 
    private Vector2 moveDirection; 
    private Collider2D currentFileCollider = null;
    private Enemy1111AnimationController animationController; // 动画控制器
    private bool isPaused = false;  // 标志，是否暂停敌人移动
    private Transform fileTransform; // 文件的位置
    private bool isPickingUpFile = false; // 是否正在捡起文件
    // 存储多个 Box 和 Folder 的位置
    private Dictionary<int, Vector2> detectedBoxes = new Dictionary<int, Vector2>();
    private Dictionary<int, Vector2> detectedFolders = new Dictionary<int, Vector2>();

    public static EnemyController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    void Start()
    {
        animationController = GetComponent<Enemy1111AnimationController>();
        foreach (FlowPath flow in flows)
        {
            taskQueue.Enqueue(flow);
        }

        StartNextTask();
    }

    void Update()
    {
        // 如果游戏失败，停止敌人移动
        if (isGameFailed || isPickingUpFile)
            return;

        // 移动到下一个路径点并更新方向
        MoveToNextWaypoint();
        float currentAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;  // 计算当前角度
        animationController.UpdateAnimation(moveDirection, currentAngle);

        // 检查文件夹、箱子、玩家和墙壁
        CheckForFolderInView();
        CheckForBoxInView();
        CheckForPlayerInSightRange();
        CheckForWallInHearingRange();
    }
    private void StartNextTask()
    {
        if (taskQueue.Count > 0)
        {
            FlowPath nextFlow = taskQueue.Dequeue();
            currentWaypointIndex = 0;
            // 处理该流程的路径和等待时间等
        }
        else
        {
            Debug.Log("No more tasks in queue.");
        }
    }
    private void MoveToNextWaypoint()
    {
        if (isExecuting) return;

        if (currentWaypointIndex >= flows[0].waypoints.Length)
            return;

        // 获取当前目标路径点
        Transform targetWaypoint = flows[0].waypoints[currentWaypointIndex];
        Vector3 targetPosition = targetWaypoint.position;

        // 计算移动方向
        moveDirection = (targetPosition - transform.position).normalized;

        // 移动敌人
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
        {
            currentWaypointIndex++;
        }
    }

    private void StartPickUpFileAnimation(string direction)
    {
        if (isPickingUpFile) return; // 如果已经在捡文件，则不再执行

        isPickingUpFile = true;  // 设置为正在捡文件状态
      // 暂停路径行走

        // 执行蹲下捡文件动画
      
        // 假设捡文件动画需要4秒钟
        StartCoroutine(WaitForPickUpAnimationToEnd());
    }

    private IEnumerator WaitForPickUpAnimationToEnd()
    {
        // 捡文件动画播放完成后恢复行走
        yield return new WaitForSeconds(4f);

        isPickingUpFile = false;  // 重置为未捡文件状态
    }

   

   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Folder"))
        {
            currentFileCollider = other;  // 记录当前碰撞的文件物体

            // 获取与文件夹的碰撞方向
            Vector2 directionToFolder = (other.transform.position - transform.position).normalized;
            float angleToFolder = Vector2.Angle(Vector2.right, directionToFolder); // 计算夹角

            // 触发蹲下捡起文件的动画
            if (angleToFolder >= -45f && angleToFolder <= 45f)
            {
                StartPickUpFileAnimation("Right");
            }
            else if (angleToFolder >= 135f || angleToFolder <= -135f)
            {
                StartPickUpFileAnimation("Left");
            }
            else if (angleToFolder > 45f && angleToFolder < 135f)
            {
                StartPickUpFileAnimation("Up");
            }
            else if (angleToFolder < -45f && angleToFolder > -135f)
            {
                StartPickUpFileAnimation("Down");
            }
        }
    }
    public void OnFilePickedUp()
    {
        Debug.Log("文件捡起完毕，恢复移动。");
        isPaused = false;  // 恢复敌人移动
        StartNextTask();  // 恢复流程，继续执行下一个任务
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Folder"))
        {
            // 当离开文件碰撞体时，恢复路径行走并触发行走动画
            if (isPickingUpFile)
            {
                isPickingUpFile = false; // 恢复正常状态
             // 恢复路径行走
                float currentAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                animationController.UpdateAnimation(moveDirection, currentAngle);// 停止捡文件动画，恢复空闲或行走状态
            }
        }
    }

    private void CheckForFolderInView()
    {
        if (isGameFailed) return;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, fieldOfViewDistance, folderLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Folder"))
            {
                Vector2 folderPosition = hitCollider.transform.position;
                Vector2 toFolder = (folderPosition - (Vector2)transform.position).normalized;

                // 计算敌人与文件夹的夹角
                float angleToFolder = Vector2.Angle(moveDirection, toFolder);

                // 检查是否在视野角度内
                if (angleToFolder < fieldOfViewAngle / 2f)
                {
                    int folderID = hitCollider.GetInstanceID();

                    if (!detectedFolders.ContainsKey(folderID))
                    {
                        detectedFolders[folderID] = folderPosition;

                        // 根据夹角选择相应的蹲下捡起动画
                        if (angleToFolder >= -45f && angleToFolder <= 45f)
                        {
                            animationController.SetCrouchPickUpRightAnimation();
                        }
                        else if (angleToFolder >= 135f || angleToFolder <= -135f)
                        {
                            animationController.SetCrouchPickUpLeftAnimation();
                        }
                        else if (angleToFolder > 45f && angleToFolder < 135f)
                        {
                            animationController.SetCrouchPickUpUpAnimation();
                        }
                        else if (angleToFolder < -45f && angleToFolder > -135f)
                        {
                            animationController.SetCrouchPickUpDownAnimation();
                        }
                    }
                }
            }
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
                    // 第一次检测到这个 Box
                    int boxID = hitCollider.GetInstanceID(); //获取id
                    detectedBoxes[boxID] = boxPosition;
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
                GameVictory(); // 游戏胜利
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


