using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Door doorController; // ���� DoorController2D �ű�
    public Possessed possessed;
    public Key Key;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("dreamer"))  // ������������봥��������
        {
            if (Key.getKey)
                doorController.ToggleDoor(); // �л��ŵ�״̬
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("dreamer"))  // ���������뿪����������
        {
            doorController.ToggleDoor(); // �л��ŵ�״̬
        }
    }
}
