using UnityEngine;

public class TriggerToSpawnKiller : MonoBehaviour
{
    public GameObject killerPrefab; // 引用Killer物体的Prefab
    public Transform spawnLocation; // 生成位置（可设置为空，则在触发器处生成）

    private bool hasTriggered = false; // 防止重复触发

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查触发对象是否是玩家
        if (collision.CompareTag("dreamer") && !hasTriggered)
        {
            hasTriggered = true; // 确保只触发一次

            // 如果有指定生成位置，则在该位置生成物体，否则在触发器位置生成
            Vector3 spawnPosition = spawnLocation != null ? spawnLocation.position : transform.position;

            // 生成Killer物体
            GameObject killer = Instantiate(killerPrefab, spawnPosition, Quaternion.identity);

            // 确保生成物体的Tag为"Killer"
            killer.tag = "killer";

            Debug.Log("Killer已生成！");
        }
    }
}
