using UnityEngine;

public class TriggerToSpawnSpy : MonoBehaviour
{
    public GameObject uPrefab; // ����Killer�����Prefab
    public Transform spawnLocation; // 
    public float detectionRadius;  // Բ�μ�ⷶΧ�İ뾶
    public LayerMask spyLayer;       // ������ڵĲ㣨����ͨ�� Inspector ���ã�
    public bool isspyInRange; // ����Ƿ��ڼ�ⷶΧ��

    private bool hasTriggered; // ��ֹ�ظ�����

    private void Start()
    {
        hasTriggered = false;
        isspyInRange = false;

    }

    private void Update()
    {
        CheckDreamerInRange();
    }


    void CheckDreamerInRange()
    {
        // ʹ�� Physics2D.OverlapCircle ���Բ�η�Χ�ڵ�����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, spyLayer);

        if (colliders.Length > 0)
        {
            if (!hasTriggered)
            {
                hasTriggered = true; // ȷ��ֻ����һ��


                Vector3 spawnPosition = spawnLocation != null ? spawnLocation.position : transform.position;

                // ����Killer����
                GameObject killer = Instantiate(uPrefab, spawnPosition, Quaternion.identity);

                // ȷ�����������TagΪ"Killer"
                killer.tag = "u";

                Debug.Log("u�����ɣ�");
                
            }
        }

    }
}