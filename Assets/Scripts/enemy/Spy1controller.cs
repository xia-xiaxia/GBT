using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sky1controller : MonoBehaviour
{
    public Transform[] waypoints; // 路径点数组
    public float moveSpeed = 2f; // 敌人移动速度
    private Transform targetWaypoint;
    public bool isExecuting = false;
    public float fieldOfViewDistance = 5f; // 视野范围
    public float fieldOfViewAngle = 110f; // 视野角度
    public float hearingRange = 5f;
    private Vector2 lastBoxPosition = Vector2.zero; // 记录上次检测到的 Box 位置
    private bool hasDetectedBox = false; // 标记是否已经检测到 Box
    public bool isGameFailed = false; // 游戏是否失败
    public bool isHit = false;   // 是否撞击墙壁发出声音
    private int currentWaypointIndex = 0; // 当前路径点索引
    public LayerMask playerLayer;   // 玩家所在的层
    public LayerMask obstacleLayer; // 障碍物（可交互）所在的层
    private Vector2 moveDirection; // 当前移动方向                        
    private SpyAnimationController animationController;
    // 存储多个 Box 的位置
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

        // 移动到目标航点
        Vector2 direction = targetWaypoint.position - transform.position;
        moveDirection = direction.normalized; 

        // 计算当前角度
        float currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);
        //是否到达
        if ((Vector2)transform.position == (Vector2)targetWaypoint.position)
        {
            currentWaypointIndex++;
            Debug.Log(currentWaypointIndex);

            //到达最后一个点的时候，停止移动
            if (currentWaypointIndex >= waypoints.Length)
            {
                enabled = false;
                animationController.SetIdleAnimation();
                return;
            }

            // 继续移动到下一个路径点
            targetWaypoint = waypoints[currentWaypointIndex];
        }

        // 根据角度判断动画的逻辑
        animationController.UpdateAnimation(moveDirection, currentAngle);
        CheckForBoxInView();
        CheckForPlayerInSightRange();
        CheckForWallInHearingRange();
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
                        int boxID = hitCollider.GetInstanceID(); //获取id

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
                        animationController.SetIdleAnimation();
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
            animationController.SetIdleAnimation();
            GameVictory(); // 游戏胜利
        }
    } 
     private void GameVictory()
    {
        isGameFailed = false;
        Debug.Log("阻止了坏结局，游戏胜利！");
        animationController.SetIdleAnimation();
      
    
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