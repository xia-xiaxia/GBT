using UnityEngine;

public class Enemy1111AnimationController : MonoBehaviour
{
    private Animator animator;

    // Define animation parameters (same as before)
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsForwardWalking = Animator.StringToHash("IsForwardWalking");
    private static readonly int IsBackWalking = Animator.StringToHash("IsBackWalking");
    private static readonly int IsRightWalking = Animator.StringToHash("IsRightWalking");
    private static readonly int IsLeftWalking = Animator.StringToHash("IsLeftWalking");

    private static readonly int IsCrouchPickUpRight = Animator.StringToHash("IsCrouchPickUpRight");
    private static readonly int IsCrouchPickUpLeft = Animator.StringToHash("IsCrouchPickUpLeft");
    private static readonly int IsCrouchPickUpUp = Animator.StringToHash("IsCrouchPickUpUp");
    private static readonly int IsCrouchPickUpDown = Animator.StringToHash("IsCrouchPickUpDown");

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update animation based on movement direction
    public void UpdateAnimation(Vector2 moveDirection, float angle)
    {
        animator.SetBool(IsWalking, false);
        animator.SetBool(IsForwardWalking, false);
        animator.SetBool(IsBackWalking, false);
        animator.SetBool(IsRightWalking, false);
        animator.SetBool(IsLeftWalking, false);


        // 如果当前敌人有移动方向
        if (moveDirection != Vector2.zero)
        {
            animator.SetBool(IsWalking, true); 
            // 角度判断逻辑
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
    public void SetCrouchPickUpRightAnimation()
    {
        
        animator.Play("CrouchPickUpRight");

    }

    public void SetCrouchPickUpLeftAnimation()
    {
        animator.SetTrigger("crouchPickUpLeft");
    }

    public void SetCrouchPickUpUpAnimation()
    {
        animator.SetTrigger("crouchPickUpUp");
    }

    public void SetCrouchPickUpDownAnimation()
    {
        animator.SetTrigger("crouchPickUpDown");
    }
    // Set crouch pick-up animation based on direction

    public void SetIdleAnimation()
    {
        animator.SetBool(IsCrouchPickUpRight, false);
        animator.SetBool(IsCrouchPickUpLeft, false);
        animator.SetBool(IsCrouchPickUpUp, false);
        animator.SetBool(IsCrouchPickUpDown, false);
        animator.SetBool(IsWalking, false);
        animator.SetBool(IsBackWalking, false);
        animator.SetBool(IsForwardWalking, false);
        animator.SetBool(IsRightWalking, false);
        animator.SetBool(IsLeftWalking, false);

    }
    /*public void SetPickUpAnimation()
    {

        animator.Play("CrouchPickUpRight");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (stateInfo.normalizedTime >1.0f) {

            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        }
    }*/
}