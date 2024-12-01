using UnityEngine;

public class TriggerToSpawnSpy : MonoBehaviour
{
    public GameObject uPrefab; // 引用Killer物体的Prefab
    public Transform spawnLocation; // 
    public float detectionRadius;  // 圆形检测范围的半径
    public LayerMask spyLayer;       // 玩家所在的层（可以通过 Inspector 设置）
    public bool isspyInRange; // 玩家是否在检测范围内

    private bool hasTriggered; // 防止重复触发

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
        // 使用 Physics2D.OverlapCircle 检测圆形范围内的物体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, spyLayer);

        if (colliders.Length > 0)
        {
            if (!hasTriggered)
            {
                hasTriggered = true; // 确保只触发一次


                Vector3 spawnPosition = spawnLocation != null ? spawnLocation.position : transform.position;

                // 生成Killer物体
                GameObject killer = Instantiate(uPrefab, spawnPosition, Quaternion.identity);

                // 确保生成物体的Tag为"Killer"
                killer.tag = "u";

                Debug.Log("u已生成！");
                
            }
        }

    }
}