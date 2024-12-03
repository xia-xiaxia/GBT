using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSecond : MonoBehaviour
{
    public bool isWin; 
    public bool isFailed;
    public EnemyFlowController enemyFlowController;
    public GameObject agameObject;
    public float detectionRadius;  // 圆形检测范围的半径
    public LayerMask dreamerLayer;       
    public bool isdreamerInRange = false; // 玩家是否在检测范围内
    public PickupUUU Pickupuuu;

    public static GameSecond Instance { get; private set; }
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
        isFailed = enemyFlowController.isGameFailed;
        StartCoroutine(Nm());
        if (isFailed)
        {
            agameObject.SetActive(false);
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

    IEnumerator Nm()
    {
        yield return new WaitForSeconds(40f);
        CheckKeyInRange();
    }
}