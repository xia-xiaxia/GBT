using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameThird : MonoBehaviour
{
    public bool isWin; 
    public bool isFaild;
    public EnemyController enemyController;
    public GameObject gameObject;
    public float detectionRadius;  // Բ�μ�ⷶΧ�İ뾶
    public LayerMask dreamerLayer;       // ������ڵĲ�
    public bool isdreamerInRange = false; // ����Ƿ��ڼ�ⷶΧ��

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
        // ʹ�� Physics2D.OverlapCircle ���Բ�η�Χ�ڵ�����
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