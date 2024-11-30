using UnityEngine;

public class PickupU: MonoBehaviour
{
    public float detectionRadius = 0.2f;  // Բ�μ�ⷶΧ�İ뾶
    public LayerMask dreamerLayer;       // ������ڵĲ㣨����ͨ�� Inspector ���ã�
    public bool isspyInRange = false;
    //���������u�̷�����ײ��u�̾ͻ���ʧ��ʵ��Ч���Ǳ�����

    private void Start()
    {
        gameObject.SetActive(true);
    }
    private void Update()
    {

        CheckKeyInRange();
    
    }


    void CheckKeyInRange()
    {
        // ʹ�� Physics2D.OverlapCircle ���Բ�η�Χ�ڵ�����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, dreamerLayer);

        if (colliders.Length > 0)
        {
            gameObject.SetActive(false);

        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}

