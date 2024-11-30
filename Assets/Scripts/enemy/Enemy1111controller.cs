using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    [System.Serializable]
    public class FlowPath
    {
        public string flowName; // ��������
        public Transform[] waypoints; // ·��������
        public float waitTimeAtPoint = 0f; // ÿ����ĵȴ�ʱ��
    }

    public FlowPath[] flows; // ��������
    public float moveSpeed = 0.5f; // �����ƶ��ٶ�
    private Queue<FlowPath> taskQueue = new Queue<FlowPath>(); // �������
    public bool isExecuting = false;
    public float fieldOfViewDistance = 5f; // ��Ұ��Χ
    public float fieldOfViewAngle = 110f;
    public float hearingRange = 5f;// ��Ұ�Ƕ�
    private Vector2 lastBoxPosition = Vector2.zero; // ��¼�ϴμ�⵽�� Box λ��
    private bool hasDetectedBox = false; // ����Ƿ��Ѿ���⵽ Box
    private bool isGameFailed = false;
    public bool isHit = false;// ��Ϸ�Ƿ�ʧ��
    private int currentWaypointIndex = 0; // ��ǰ·��������
    public LayerMask playerLayer;   // ������ڵĲ�
    public LayerMask obstacleLayer;
    public LayerMask folderLayer; 
    private Vector2 moveDirection; 
    private Collider2D currentFileCollider = null;
    private Enemy1111AnimationController animationController; // ����������
    private bool isPaused = false;  // ��־���Ƿ���ͣ�����ƶ�
    private Transform fileTransform; // �ļ���λ��
    private bool isPickingUpFile = false; // �Ƿ����ڼ����ļ�
    // �洢��� Box �� Folder ��λ��
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
        // �����Ϸʧ�ܣ�ֹͣ�����ƶ�
        if (isGameFailed || isPickingUpFile)
            return;

        // �ƶ�����һ��·���㲢���·���
        MoveToNextWaypoint();
        float currentAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;  // ���㵱ǰ�Ƕ�
        animationController.UpdateAnimation(moveDirection, currentAngle);
        CheckForBoxInView();
        CheckForPlayerInSightRange();
        CheckForWallInHearingRange();
    }
    private IEnumerator ExecuteFlow(FlowPath flow)
    {
        isExecuting = true;
        Debug.Log($"��ʼ���̣�{flow.flowName}");

        // ִ�������е�����·����
        for (int i = 0; i < flow.waypoints.Length; i++)
        {
            Transform waypoint = flow.waypoints[i];

            // �ƶ�����ǰ·����
            yield return MoveToWaypoint(waypoint.position);

            // �ȴ�ָ��ʱ��
            yield return new WaitForSeconds(flow.waitTimeAtPoint);

            // �������ָ���ĵ㣬�л����������磺��6���㣩
            /*f (i == 5) // ����ͨ��public����������
             {
                 // �ڵ�6����ִ�����⶯�����������
                 animationController.SetCrouchAnimation();
             }*/
        }

        isExecuting = false;
        Debug.Log($"������̣�{flow.flowName}");

        // ��Ϸʧ�ܻ�������ɺ��л�����һ������
        StartNextTask();
    }
    private IEnumerator MoveToWaypoint(Vector3 target)
    {
        moveDirection = (target - transform.position).normalized;

        // ���ƶ���Ŀ���
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // ����Ŀ���
        transform.position = target;
    }

    private void StartNextTask()
    {
        // �����������������̣�ִ����һ������
        if (taskQueue.Count > 0)
        {
            FlowPath currentFlow = taskQueue.Dequeue(); // ��ȡ�����еĵ�һ������
            StartCoroutine(ExecuteFlow(currentFlow)); // ������ǰ����
        }
        else
        {
            Debug.Log("����������ɣ���Ϸ������");
            animationController.SetIdleAnimation();
            GameFailed(); // ������Ϸ����
        }
    }


    private void MoveToNextWaypoint()
    {
        if (isExecuting) return;

        if (currentWaypointIndex >= flows[0].waypoints.Length)
        {

            return;
        }

        // ��ȡ��ǰĿ��·����
        Transform targetWaypoint = flows[0].waypoints[currentWaypointIndex];
        Vector3 targetPosition = targetWaypoint.position;

        // �����ƶ�����
        moveDirection = (targetPosition - transform.position).normalized;

        // �ƶ�����
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
        {
            currentWaypointIndex++;
            Debug.Log(currentWaypointIndex);
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
                    // ��һ�μ�⵽��� Box
                    int boxID = hitCollider.GetInstanceID(); //��ȡid
                    detectedBoxes[boxID] = boxPosition;
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
                GameVictory(); // ��Ϸʤ��
            }
      

        }
   /* public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Folder"))
        {
            StopAllCoroutines();
            animationController.StopAllCoroutines();
            animationController.SetCrouchPickUpRightAnimation();
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Folder"))
        {
            animationController.SetIdleAnimation();
            StartNextTask();
        }
    }*/

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
            if (animationController != null)
            {
                animationController.SetIdleAnimation(); // �ö���תΪ��ֹ״̬
            }
            StopAllCoroutines();
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


