using System.Collections;
using UnityEngine;

public class PickupUUU : MonoBehaviour
{
    public float detectionRadius;  // Բ�μ�ⷶΧ�İ뾶
    public LayerMask dreamerLayer;       // ������ڵĲ㣨����ͨ�� Inspector ���ã�
    public bool isspyInRange;

    public GameObject U;
    private Renderer arenderer;


    private void Start()
    {
        if (U == null)
        {
           //Debug.LogError("U δ��ֵ������ Inspector ��ָ��һ����Ч�� GameObject��");
            return; // ��� U Ϊ�գ��Ͳ�ִ������Ĵ���
        }
        arenderer = U.GetComponent<Renderer>();
        isspyInRange = false;
        arenderer.enabled = false;
    }
    private void Update()
    {
        
            if (U == null)
            {
                //Debug.LogError("U δ��ֵ������ Inspector ��ָ��һ����Ч�� GameObject��");
                return; // ��� U Ϊ�գ��Ͳ�ִ������Ĵ���
            }
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
            U.SetActive(false);
        }
     
    }

}