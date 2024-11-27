using UnityEngine;

public class TriggerToSpawnKiller : MonoBehaviour
{
    public GameObject killerPrefab; // ����Killer�����Prefab
    public Transform spawnLocation; // ����λ�ã�������Ϊ�գ����ڴ����������ɣ�

    private bool hasTriggered = false; // ��ֹ�ظ�����

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ��鴥�������Ƿ������
        if (collision.CompareTag("dreamer") && !hasTriggered)
        {
            hasTriggered = true; // ȷ��ֻ����һ��

            // �����ָ������λ�ã����ڸ�λ���������壬�����ڴ�����λ������
            Vector3 spawnPosition = spawnLocation != null ? spawnLocation.position : transform.position;

            // ����Killer����
            GameObject killer = Instantiate(killerPrefab, spawnPosition, Quaternion.identity);

            // ȷ�����������TagΪ"Killer"
            killer.tag = "killer";

            Debug.Log("Killer�����ɣ�");
        }
    }
}
