using System.Collections;
using UnityEngine;

public class PickupUUU : MonoBehaviour
{
    public float detectionRadius;  // Բ�μ�ⷶΧ�İ뾶
    public LayerMask dreamerLayer;       // ������ڵĲ㣨����ͨ�� Inspector ���ã�
    public bool isspyInRange;
    //���������u�̷�����ײ��u�̾ͻ���ʧ��ʵ��Ч���Ǳ�����
    public bool getU;
    public GameObject U;
    private Renderer arenderer;


    private void Start()
    {
        arenderer = U.GetComponent<Renderer>();
        isspyInRange = false;
        arenderer.enabled = false;
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
            getU = true;
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
            U.SetActive(false);
        }
     
    }

}