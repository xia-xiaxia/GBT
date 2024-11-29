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

        // ����ļ��С����ӡ���Һ�ǽ��
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
            // ��������̵�·���͵ȴ�ʱ���
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
        }
    }

    private void StartPickUpFileAnimation(string direction)
    {
        if (isPickingUpFile) return; // ����Ѿ��ڼ��ļ�������ִ��

        isPickingUpFile = true;  // ����Ϊ���ڼ��ļ�״̬
      // ��ͣ·������

        // ִ�ж��¼��ļ�����
      
        // ������ļ�������Ҫ4����
        StartCoroutine(WaitForPickUpAnimationToEnd());
    }

    private IEnumerator WaitForPickUpAnimationToEnd()
    {
        // ���ļ�����������ɺ�ָ�����
        yield return new WaitForSeconds(4f);

        isPickingUpFile = false;  // ����Ϊδ���ļ�״̬
    }

   

   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Folder"))
        {
            currentFileCollider = other;  // ��¼��ǰ��ײ���ļ�����

            // ��ȡ���ļ��е���ײ����
            Vector2 directionToFolder = (other.transform.position - transform.position).normalized;
            float angleToFolder = Vector2.Angle(Vector2.right, directionToFolder); // ����н�

            // �������¼����ļ��Ķ���
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
        Debug.Log("�ļ�������ϣ��ָ��ƶ���");
        isPaused = false;  // �ָ������ƶ�
        StartNextTask();  // �ָ����̣�����ִ����һ������
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Folder"))
        {
            // ���뿪�ļ���ײ��ʱ���ָ�·�����߲��������߶���
            if (isPickingUpFile)
            {
                isPickingUpFile = false; // �ָ�����״̬
             // �ָ�·������
                float currentAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                animationController.UpdateAnimation(moveDirection, currentAngle);// ֹͣ���ļ��������ָ����л�����״̬
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

                // ����������ļ��еļн�
                float angleToFolder = Vector2.Angle(moveDirection, toFolder);

                // ����Ƿ�����Ұ�Ƕ���
                if (angleToFolder < fieldOfViewAngle / 2f)
                {
                    int folderID = hitCollider.GetInstanceID();

                    if (!detectedFolders.ContainsKey(folderID))
                    {
                        detectedFolders[folderID] = folderPosition;

                        // ���ݼн�ѡ����Ӧ�Ķ��¼��𶯻�
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


