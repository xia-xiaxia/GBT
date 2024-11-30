using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    public GameObject usbDrive;  
    public Transform spawnPoint; 
    void Start()
    {
        // 初始时隐藏U盘物体
        if (usbDrive != null)
        {
            usbDrive.SetActive(false);
        }
    }

    // 当物体进入触发器时
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查物体是否具有Sky标签
        if (other.CompareTag("Spy"))
        {
            // 设置 U盘物体的位置为指定的 spawnPoint
            if (usbDrive != null && spawnPoint != null)
            {
                usbDrive.transform.position = spawnPoint.position; // 将 U盘物体放置到指定位置
                usbDrive.SetActive(true);  // 显示 U盘物体
            }
        }
    }


    
   
}
