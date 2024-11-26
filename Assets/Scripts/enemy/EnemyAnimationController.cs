using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // ��ȡAnimator���
        animator = GetComponent<Animator>();
    }

    // ���¶���״̬
    public void UpdateAnimation(Vector2 moveDirection)
    {
        if (moveDirection.sqrMagnitude > 0f)
        {
            // �����ƶ�����������˵ĳ��򶯻�
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

            if (angle >= -45f && angle < 45f) // ��ǰ
            {
                SetWalkingAnimation("Walk_Forward");
            }
            else if (angle >= 45f && angle < 135f) // ���Ҳ�
            {
                SetWalkingAnimation("Walk_Side");
            }
            else if (angle <= -45f && angle > -135f) // �����
            {
                SetWalkingAnimation("Walk_Side");
            }
            else // ������
            {
                SetWalkingAnimation("Walk_Back");
            }
        }
        else
        {
            // ���û���ƶ������ž�ֹ����
            SetWalkingAnimation("Idle");
        }
    }

    // �������߶���
    private void SetWalkingAnimation(string animationName)
    {
        // �����������߶���
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsSideWalking", false);
        animator.SetBool("IsBackWalking", false);
        animator.SetBool("IsForwardWalking", false);

        // ����ָ���Ķ������ƣ����ö�Ӧ�Ķ���
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
