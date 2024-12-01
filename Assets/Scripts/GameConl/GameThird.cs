using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameThird : MonoBehaviour
{
    public bool isWin;
    public bool isFaild;
    public EnemyFlowController enemyFlowController;
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
        isFaild = false;
        isWin = false;
    }

    void Update()
    {
        isFaild = enemyFlowController.isGameFailed;
        StartCoroutine(nm());
        if (isWin) isFaild = false;
        else if (isFaild)
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
            isWin = true;

        }
        else
        {
            isFaild = true;
        }
    }

    IEnumerator nm()
    {
        yield return new WaitForSeconds(30f);
        CheckKeyInRange();
    }
}