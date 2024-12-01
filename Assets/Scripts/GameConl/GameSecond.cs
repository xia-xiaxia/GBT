using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSecond : MonoBehaviour
{
    public bool isWin; 
    public bool isFaild;
    public EnemyController enemyController;
    public GameObject gameObject;
    public float detectionRadius;  // 圆形检测范围的半径
    public LayerMask dreamerLayer;       // 玩家所在的层（可以通过 Inspector 设置）
    public bool isdreamerInRange = false; // 玩家是否在检测范围内

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
        isFaild = false;
        isWin = false;
    }

    void Update()
    {
        isFaild = enemyController.isGameFailed;
        StartCoroutine(nm());
        if (isFaild)
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
            isFaild = true;

        }
        else isWin = true;

    }

    IEnumerator nm()
    {
        yield return new WaitForSeconds(40f);
        CheckKeyInRange();
    }
}