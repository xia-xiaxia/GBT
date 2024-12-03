//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GameThird : MonoBehaviour
//{
//    public bool isWin;
//    public bool isFailed;
//    public Transform SpyTransform;
//    public EnemyFlowController enemyFlowController;
//    public float detectionRadius;  // Բ�μ�ⷶΧ�İ뾶
//    public LayerMask dreamerLayer;       // ������ڵĲ㣨����ͨ�� Inspector ���ã�
//    public bool isdreamerInRange = false; // ����Ƿ��ڼ�ⷶΧ��
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
//        // ʹ�� Physics2D.OverlapCircle ���Բ�η�Χ�ڵ�����
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
    public float detectionRadius;  // Բ�μ�ⷶΧ�İ뾶
    public LayerMask dreamerLayer;       // ������ڵĲ㣨����ͨ�� Inspector ���ã�
    public bool isdreamerInRange = false; // ����Ƿ��ڼ�ⷶΧ��

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
        // ʹ�� Physics2D.OverlapCircle ���Բ�η�Χ�ڵ�����
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