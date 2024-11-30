using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    public GameObject usbDrive;  
    public Transform spawnPoint; 
    void Start()
    {
        // ��ʼʱ����U������
        if (usbDrive != null)
        {
            usbDrive.SetActive(false);
        }
    }

    // ��������봥����ʱ
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ��������Ƿ����Sky��ǩ
        if (other.CompareTag("Spy"))
        {
            // ���� U�������λ��Ϊָ���� spawnPoint
            if (usbDrive != null && spawnPoint != null)
            {
                usbDrive.transform.position = spawnPoint.position; // �� U��������õ�ָ��λ��
                usbDrive.SetActive(true);  // ��ʾ U������
            }
        }
    }


    
   
}
