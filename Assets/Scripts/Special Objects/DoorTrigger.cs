using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Possessed possessed;
    public Key Key;

    private bool isOpen; // �ŵ�״̬���򿪻�ر�
    public float rotationSpeed = 90f;  // ÿ����ת�ĽǶȣ��ȣ�
    private float targetRotation = 90f;  // Ŀ����ת�Ƕ�
    private float currentRotation = 0f;  // ��ǰ��ת�Ƕ�
    private Collider2D doorCollider;
    public float detectionRadius = 5f;  // Բ�μ�ⷶΧ�İ뾶
    public LayerMask dreamerLayer;       // ������ڵĲ㣨����ͨ�� Inspector ���ã�

    private void Start()
    {
        isOpen = false;
        doorCollider = GetComponent<Collider2D>();
    }
    private void Update()
    {
        CheckKeyInRange();
        // ����ŵĿ���״̬
        if (isOpen)
        {
            targetRotation = 90f;
            doorCollider.enabled = false;
        }
        else
        {
            targetRotation = 0f;
        }
        Rotation();
    }
    void Rotation()
    {
        if (currentRotation < targetRotation)
        {
            // ÿ֡�� Y ����תһ���Ƕ�
            float step = rotationSpeed * Time.deltaTime;
            transform.Rotate(0, step, 0); // �� Y ����ת
            currentRotation += step; // �ۼ�����ת�ĽǶ�

            // ȷ����ǰ��ת�ǶȲ��ᳬ��Ŀ��Ƕ�
            if (currentRotation >= targetRotation)
            {
                currentRotation = targetRotation; // ����ΪĿ��Ƕ�
                doorCollider.enabled = false;

            }
        }
        if (currentRotation < targetRotation)
        {
            // ÿ֡�� Y ����תһ���Ƕ�
            float step = rotationSpeed * Time.deltaTime;
            transform.Rotate(0, step, 0); // �� Y ����ת
            currentRotation -= step; // �ۼ�����ת�ĽǶ�

            // ȷ����ǰ��ת�ǶȲ��ᳬ��Ŀ��Ƕ�
            if (currentRotation <= targetRotation)
            {
                currentRotation = targetRotation; // ����ΪĿ��Ƕ�
            }
        }
    }

    void CheckKeyInRange()
    {
        // ʹ�� Physics2D.OverlapCircle ���Բ�η�Χ�ڵ�����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, dreamerLayer);

        if (colliders.Length > 0)
        {
            if(Key.getKey)
            {
                isOpen = true;
            }
        }
    }
}
