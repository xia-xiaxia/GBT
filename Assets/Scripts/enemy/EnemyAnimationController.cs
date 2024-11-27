using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // 获取Animator组件
        animator = GetComponent<Animator>();
    }

    // 更新动画状态
    public void UpdateAnimation(Vector2 moveDirection)
    {
        if (moveDirection.sqrMagnitude > 0f)
        {
            // 根据移动方向决定敌人的朝向动画
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

            if (angle >= -45f && angle < 45f) // 朝前
            {
                SetWalkingAnimation("Walk_Forward");
            }
            else if (angle >= 45f && angle < 135f) // 朝右侧
            {
                SetWalkingAnimation("Walk_Side");
            }
            else if (angle <= -45f && angle > -135f) // 朝左侧
            {
                SetWalkingAnimation("Walk_Side");
            }
            else // 背对走
            {
                SetWalkingAnimation("Walk_Back");
            }
        }
        else
        {
            // 如果没有移动，播放静止动画
            SetWalkingAnimation("Idle");
        }
    }

    // 设置行走动画
    private void SetWalkingAnimation(string animationName)
    {
        // 重置所有行走动画
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsSideWalking", false);
        animator.SetBool("IsBackWalking", false);
        animator.SetBool("IsForwardWalking", false);

        // 根据指定的动画名称，启用对应的动画
        switch (animationName)
        {
            case "Walk_Forward":
                animator.SetBool("IsForwardWalking", true);
                break;
            case "Walk_Side":
                animator.SetBool("IsSideWalking", true);
                break;
            case "Walk_Back":
                animator.SetBool("IsBackWalking", true);
                break;
            case "Idle":
                animator.SetBool("IsWalking", false);
                break;
        }
    }
}
