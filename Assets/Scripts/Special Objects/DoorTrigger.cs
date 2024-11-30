using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Possessed possessed;
    public Key Key;

    private bool isOpen; // 门的状态，打开或关闭
    public float rotationSpeed = 90f;  // 每秒旋转的角度（度）
    private float targetRotation = 90f;  // 目标旋转角度
    private float currentRotation = 0f;  // 当前旋转角度
    private Collider2D doorCollider;
    public float detectionRadius = 5f;  // 圆形检测范围的半径
    public LayerMask dreamerLayer;       // 玩家所在的层（可以通过 Inspector 设置）

    private void Start()
    {
        isOpen = false;
        doorCollider = GetComponent<Collider2D>();
    }
    private void Update()
    {
        CheckKeyInRange();
        // 检测门的开关状态
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
            // 每帧绕 Y 轴旋转一定角度
            float step = rotationSpeed * Time.deltaTime;
            transform.Rotate(0, step, 0); // 绕 Y 轴旋转
            currentRotation += step; // 累加已旋转的角度

            // 确保当前旋转角度不会超过目标角度
            if (currentRotation >= targetRotation)
            {
                currentRotation = targetRotation; // 设置为目标角度
                doorCollider.enabled = false;

            }
        }
        if (currentRotation < targetRotation)
        {
            // 每帧绕 Y 轴旋转一定角度
            float step = rotationSpeed * Time.deltaTime;
            transform.Rotate(0, step, 0); // 绕 Y 轴旋转
            currentRotation -= step; // 累加已旋转的角度

            // 确保当前旋转角度不会超过目标角度
            if (currentRotation <= targetRotation)
            {
                currentRotation = targetRotation; // 设置为目标角度
            }
        }
    }

    void CheckKeyInRange()
    {
        // 使用 Physics2D.OverlapCircle 检测圆形范围内的物体
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
