using UnityEngine;

public class PickupU: MonoBehaviour
{
  //���������u�̷�����ײ��u�̾ͻ���ʧ��ʵ��Ч���Ǳ�����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision Detected with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("u"))
        {
            
            Destroy(collision.gameObject);//����u��
            Debug.Log("Destroying object: " + collision.gameObject.name); // ��ӡ��־��ȷ������
        }
    }

   
}

