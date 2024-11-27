using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;

    // ���ﶨ����Ƕ������������������������
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsForwardWalking = Animator.StringToHash("IsForwardWalking");
    private static readonly int IsBackWalking = Animator.StringToHash("IsBackWalking");
    private static readonly int IsRightWalking = Animator.StringToHash("IsRightWalking");
    private static readonly int IsLeftWalking = Animator.StringToHash("IsLeftWalking");

    public float turnSpeed = 5f; // ��ת��ƽ���ٶȣ�ֵԽ��Խƽ��

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // ���¶���״̬�ķ���
    public void UpdateAnimation(Vector2 moveDirection, float currentAngle)
    {
        // �Ȱ����е�����״̬���Ϊ false
        animator.SetBool(IsWalking, false);
        animator.SetBool(IsForwardWalking, false);
        animator.SetBool(IsBackWalking, false);
        animator.SetBool(IsRightWalking, false);
        animator.SetBool(IsLeftWalking, false);

        // �����ǰ�������ƶ�����
        // �����ǰ�������ƶ�����
        if (moveDirection != Vector2.zero)
        {
            animator.SetBool(IsWalking, true); // ����Ϊ����״̬

            // ������ˮƽ X ��֮��ĽǶ�
            float angle = currentAngle; // ֱ��ʹ�õ�ǰ�Ƕȣ����� `transform.eulerAngles.z` �ṩ��
            //Debug.Log($"MoveDirection: {moveDirection}, Angle: {angle}");

            // �Ƕ����֣�ȷ��������ȷ
            if (angle >= -45f && angle <= 45f) // ����
            {
                animator.SetBool(IsRightWalking, true);
            }
            else if (angle >= 135f || angle <= -135f) // ����
            {
                animator.SetBool(IsLeftWalking, true);
            }
            else if (angle > 45f && angle < 135f) // ���ϣ���������
            {
                animator.SetBool(IsBackWalking, true);
            }
            else if (angle < -45f && angle > -135f) // ���£���������
            {
                animator.SetBool(IsForwardWalking, true);
            }
        }
    }
}