using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isOpen = false; // 门的状态，打开或关闭
    public GameObject door;
    public float rotationSpeed = 90f;  // 每秒旋转的角度（度）
    private float targetRotation = 90f;  // 目标旋转角度
    private float currentRotation = 0f;  // 当前旋转角度
    private Collider2D doorCollider;

    private void Start()
    {
        doorCollider = door.GetComponent<Collider2D>();
    }
    void Update()
    {
        // 检测门的开关状态
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

    // 切换门的开关状态
    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }

    void Rotation()
    {
        if (currentRotation < targetRotation)
        {
            // 每帧绕 Y 轴旋转一定角度
            float step = rotationSpeed * Time.deltaTime;
            door.transform.Rotate(0, step, 0); // 绕 Y 轴旋转
            currentRotation += step; // 累加已旋转的角度

            // 确保当前旋转角度不会超过目标角度
            if (currentRotation >= targetRotation)
            {
                currentRotation = targetRotation; // 设置为目标角度
                doorCollider.enabled = false;

            }
        }
        if(currentRotation < targetRotation)
        {
            // 每帧绕 Y 轴旋转一定角度
            float step = rotationSpeed * Time.deltaTime;
            door.transform.Rotate(0, step, 0); // 绕 Y 轴旋转
            currentRotation -= step; // 累加已旋转的角度

            // 确保当前旋转角度不会超过目标角度
            if (currentRotation <= targetRotation)
            {
                currentRotation = targetRotation; // 设置为目标角度
            }
        }
    }
}
