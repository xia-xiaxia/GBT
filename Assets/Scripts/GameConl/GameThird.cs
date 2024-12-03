//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GameThird : MonoBehaviour
//{
//    public bool isWin;
//    public bool isFailed;
//    public Transform SpyTransform;
//    public EnemyFlowController enemyFlowController;
//    public float detectionRadius;  // 圆形检测范围的半径
//    public LayerMask dreamerLayer;       // 玩家所在的层（可以通过 Inspector 设置）
//    public bool isdreamerInRange = false; // 玩家是否在检测范围内
//    public Transform U;
//    public PickupUUU Pickupuuu;

//    public static GameThird Instance { get; private set; }
//    private void Awake()
//    {
//        if (Instance != null && Instance != this)
//        {
//            Destroy(gameObject);
//        }
//        Instance = this;
//    }
//    void Start()
//    {
//        isWin = false;
//        isFailed = false;
//    }

//    void Update()
//    {
//        isFailed = enemyFlowController.isGameFailed;
//        CheckKeyInRange();
//        if (isWin) isFailed = false;
//    }

//    void CheckKeyInRange()
//    {
//        // 使用 Physics2D.OverlapCircle 检测圆形范围内的物体
//        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, dreamerLayer);

//        if (colliders.Length > 0)
//        {

//        }
//    }

//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameThird : MonoBehaviour
{
    public bool isWin;
    public bool isFailed;
    public EnemyController enemyController;
    public GameObject gameObject;
    public float detectionRadius;  // 圆形检测范围的半径
    public LayerMask dreamerLayer;       // 玩家所在的层（可以通过 Inspector 设置）
    public bool isdreamerInRange = false; // 玩家是否在检测范围内

    public static GameThird Instance { get; private set; }

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
        isFailed = false;
        isWin = false;
    }

    void Update()
    {
        isFailed = enemyController.isGameFailed;
        StartCoroutine(nm());
        if (isFailed)
        {
            gameObject.SetActive(false);
        }
    }

    void CheckKeyInRange()
    {
        // 使用 Physics2D.OverlapCircle 检测圆形范围内的物体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, dreamerLayer);

        if (colliders.Length > 0)
        {
            isFailed = true;

        }
        else isWin = true;

    }

    IEnumerator nm()
    {
        yield return new WaitForSeconds(40f);
        CheckKeyInRange();
    }
}