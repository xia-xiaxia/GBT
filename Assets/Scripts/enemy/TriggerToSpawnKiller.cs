using UnityEngine;

public class TriggerToSpawnKiller : MonoBehaviour
{
    public GameObject killerPrefab; // ����Killer�����Prefab
    public Transform spawnLocation; // 
    public float detectionRadius;  // Բ�μ�ⷶΧ�İ뾶
    public LayerMask dreamerLayer;       // ������ڵĲ㣨����ͨ�� Inspector ���ã�
    public bool isdreamerInRange ; // ����Ƿ��ڼ�ⷶΧ��

    private bool hasTriggered; // ��ֹ�ظ�����

    private void Start()
    {
        hasTriggered = false;
        isdreamerInRange = false;

    }

    private void Update()
    {
        CheckDreamerInRange();
    }
   

    void CheckDreamerInRange()
    {
        // ʹ�� Physics2D.OverlapCircle ���Բ�η�Χ�ڵ�����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, dreamerLayer);

        if (colliders.Length > 0)
        {
            if (!hasTriggered)
            {
                hasTriggered = true; // ȷ��ֻ����һ��


                Vector3 spawnPosition = spawnLocation != null ? spawnLocation.position : transform.position;

                // ����Killer����
                GameObject killer = Instantiate(killerPrefab, spawnPosition, Quaternion.identity);

                // ȷ�����������TagΪ"Killer"
                killer.tag = "killer";

                Debug.Log("Killer�����ɣ�");
                Debug.Log("Gameover");
            }
        }

    }
}
