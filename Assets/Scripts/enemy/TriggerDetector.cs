using UnityEngine;

public class TriggerDetector : MonoBehaviour
{

    public float detectionRadius = 0.2f;  // Բ�μ�ⷶΧ�İ뾶
    public LayerMask spyLayer;      
    public bool isspyInRange = false;
    //���������u�̷�����ײ��u�̾ͻ���ʧ��ʵ��Ч���Ǳ�����

    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void Update()
    {

        CheckKeyInRange();

    }


    void CheckKeyInRange()
    {
        // ʹ�� Physics2D.OverlapCircle ���Բ�η�Χ�ڵ�����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, spyLayer);

        if (colliders.Length > 0)
        {
            gameObject.SetActive(true);

        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
