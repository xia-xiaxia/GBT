using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSecond : MonoBehaviour
{
    public bool isWin; 
    public bool isFailed;
    public EnemyFlowController enemyFlowController;
    public GameObject agameObject;
    public float detectionRadius;  // Բ�μ�ⷶΧ�İ뾶
    public LayerMask dreamerLayer;       
    public bool isdreamerInRange = false; // ����Ƿ��ڼ�ⷶΧ��
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
        // ʹ�� Physics2D.OverlapCircle ���Բ�η�Χ�ڵ�����
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