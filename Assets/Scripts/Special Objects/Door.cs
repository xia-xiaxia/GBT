using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isOpen = false; // �ŵ�״̬���򿪻�ر�
    public GameObject door;
    public float rotationSpeed = 90f;  // ÿ����ת�ĽǶȣ��ȣ�
    private float targetRotation = 90f;  // Ŀ����ת�Ƕ�
    private float currentRotation = 0f;  // ��ǰ��ת�Ƕ�
    private Collider2D doorCollider;

    private void Start()
    {
        doorCollider = door.GetComponent<Collider2D>();
    }
    void Update()
    {
        // ����ŵĿ���״̬
        if (isOpen)
        {
            targetRotation = 90f;
        }
        else
        {
            targetRotation = 0f;
            doorCollider.enabled = true;
        }
        Rotation();
    }

    // �л��ŵĿ���״̬
    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }

    void Rotation()
    {
        if (currentRotation < targetRotation)
        {
            // ÿ֡�� Y ����תһ���Ƕ�
            float step = rotationSpeed * Time.deltaTime;
            door.transform.Rotate(0, step, 0); // �� Y ����ת
            currentRotation += step; // �ۼ�����ת�ĽǶ�

            // ȷ����ǰ��ת�ǶȲ��ᳬ��Ŀ��Ƕ�
            if (currentRotation >= targetRotation)
            {
                currentRotation = targetRotation; // ����ΪĿ��Ƕ�
                doorCollider.enabled = false;

            }
        }
        if(currentRotation < targetRotation)
        {
            // ÿ֡�� Y ����תһ���Ƕ�
            float step = rotationSpeed * Time.deltaTime;
            door.transform.Rotate(0, step, 0); // �� Y ����ת
            currentRotation -= step; // �ۼ�����ת�ĽǶ�

            // ȷ����ǰ��ת�ǶȲ��ᳬ��Ŀ��Ƕ�
            if (currentRotation <= targetRotation)
            {
                currentRotation = targetRotation; // ����ΪĿ��Ƕ�
            }
        }
    }
}
