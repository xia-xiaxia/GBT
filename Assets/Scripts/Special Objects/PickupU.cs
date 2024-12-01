using System.Collections;
using UnityEngine;

public class PickupU : MonoBehaviour
{
    public float detectionRadius;  // Բ�μ�ⷶΧ�İ뾶
    public LayerMask dreamerLayer;       // ������ڵĲ㣨����ͨ�� Inspector ���ã�
    public bool isspyInRange;
    //���������u�̷�����ײ��u�̾ͻ���ʧ��ʵ��Ч���Ǳ�����
    Renderer arenderer;

    private void Start()
    {
        arenderer = GetComponent<Renderer>();
        arenderer.enabled = false;
        isspyInRange = false;
        gameObject.SetActive(true);
    }
    private void Update()
    {
        CheckKeyInRange();
        StartCoroutine(Timer());
    }
    void CheckKeyInRange()
    {
        // ʹ�� Physics2D.OverlapCircle ���Բ�η�Χ�ڵ�����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, dreamerLayer);

        if (colliders.Length > 0)
        {
            arenderer.enabled = true;
        }
        else
        {
            arenderer.enabled = false;
        }
    }

    IEnumerator Timer()
    {
        if (arenderer.enabled)
        {
            yield return new WaitForSeconds(0.1f);
            gameObject.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
            gameObject.SetActive(true);
        }

    }
}