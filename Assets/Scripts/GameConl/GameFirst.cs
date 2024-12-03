using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFirst : MonoBehaviour
{
    public bool isWin;
    public bool isFaild;
    public EnemyFlowController enemyFlowController;
    public GameObject agameObject;
    public float detectionRadius;  // Բ�μ�ⷶΧ�İ뾶
    public LayerMask dreamerLayer;       // ������ڵĲ㣨����ͨ�� Inspector ���ã�
    public bool isdreamerInRange = false; // ����Ƿ��ڼ�ⷶΧ��

    public static GameFirst Instance { get; private set; }
    private void Awake()
    {
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
        StartCoroutine(Nm());
        if (isWin)
        {
            agameObject.SetActive(false);
            isFaild = false;
        }
        else if (isFaild)
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
            isWin = true;

        }
        else
        {
            isFaild = true;
        }
    }

    IEnumerator Nm()
    {
        yield return new WaitForSeconds(30f);
        CheckKeyInRange();
    }
}