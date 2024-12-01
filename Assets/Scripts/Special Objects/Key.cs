using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public float detectionRadius ;  // Բ�μ�ⷶΧ�İ뾶
    public LayerMask dreamerLayer;       // ������ڵĲ㣨����ͨ�� Inspector ���ã�
    public bool isdreamerInRange = false; // ����Ƿ��ڼ�ⷶΧ��
    public bool getKey;

    void Start()
    {
        getKey = false;
        gameObject.SetActive(true);
    }

    void Update()
    {
        CheckKeyInRange();
        GetKey();
    }
    void CheckKeyInRange()
    {
        // ʹ�� Physics2D.OverlapCircle ���Բ�η�Χ�ڵ�����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, dreamerLayer);

        if (colliders.Length > 0)
        {
            isdreamerInRange = true;
            getKey = true;
        }
        else
        {
            isdreamerInRange = false;
        }
    }

    void GetKey()
    {
        if (getKey)
            gameObject.SetActive(false);
    }
}
