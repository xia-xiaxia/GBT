using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;

    // 这里定义的是动画机中五个布尔参数的名称
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsForwardWalking = Animator.StringToHash("IsForwardWalking");
    private static readonly int IsBackWalking = Animator.StringToHash("IsBackWalking");
    private static readonly int IsRightWalking = Animator.StringToHash("IsRightWalking");
    private static readonly int IsLeftWalking = Animator.StringToHash("IsLeftWalking");

    public float turnSpeed = 5f; // 旋转的平滑速度，值越大越平滑

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // 更新动画状态的方法
    public void UpdateAnimation(Vector2 moveDirection, float currentAngle)
    {
        // 先把所有的行走状态标记为 false
        animator.SetBool(IsWalking, false);
        animator.SetBool(IsForwardWalking, false);
        animator.SetBool(IsBackWalking, false);
        animator.SetBool(IsRightWalking, false);
        animator.SetBool(IsLeftWalking, false);

        // 如果当前敌人有移动方向
        // 如果当前敌人有移动方向
        if (moveDirection != Vector2.zero)
        {
            animator.SetBool(IsWalking, true); // 设置为行走状态

            // 计算与水平 X 轴之间的角度
            float angle = currentAngle; // 直接使用当前角度（已由 `transform.eulerAngles.z` 提供）
            //Debug.Log($"MoveDirection: {moveDirection}, Angle: {angle}");

            // 角度区分，确保方向正确
            if (angle >= -45f && angle <= 45f) // 向右
            {
                animator.SetBool(IsRightWalking, true);
            }
            else if (angle >= 135f || angle <= -135f) // 向左
            {
                animator.SetBool(IsLeftWalking, true);
            }
            else if (angle > 45f && angle < 135f) // 向上，背对着走
            {
                animator.SetBool(IsBackWalking, true);
            }
            else if (angle < -45f && angle > -135f) // 向下，正对着走
            {
                animator.SetBool(IsForwardWalking, true);
            }
        }
    }
}