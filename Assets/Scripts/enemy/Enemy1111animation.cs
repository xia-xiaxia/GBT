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
        if (moveDirection.magnitude > 0)
        {
            animator.SetBool(IsWalking, true);
            animator.SetBool(IsForwardWalking, angle >= -45f && angle <= 45f);
            animator.SetBool(IsBackWalking, angle >= 135f || angle <= -135f);
            animator.SetBool(IsRightWalking, angle > 45f && angle < 135f);
            animator.SetBool(IsLeftWalking, angle < -45f && angle > -135f);
        }
        else
        {
            animator.SetBool(IsWalking, false);
        }
    }
    public void SetCrouchPickUpRightAnimation()
    {
        animator.SetTrigger("crouchPickUpRight");
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
}