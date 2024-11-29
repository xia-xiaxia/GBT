using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possessed : MonoBehaviour
{
    public PlayerMovement PM;
    private PossessedMove possessedMove;
    public float detectionRadius = 5f;  // Բ�μ�ⷶΧ�İ뾶
    public LayerMask playerLayer;       // ������ڵĲ㣨����ͨ�� Inspector ���ã�
    public bool isPlayerInRange = false; // ����Ƿ��ڼ�ⷶΧ��
    public GameObject Player;
    public float targetAlpha = 0.5f; // Ŀ��͸����
    private Renderer characterRenderer;
    private Collider2D characterCollider;
    public Transform transform;

    private bool IsPossessed;



    void Start()
    {
        IsPossessed = false;
        possessedMove = GetComponent<PossessedMove>();
        possessedMove.enabled = false;
        characterRenderer = Player.GetComponent<Renderer>();
        characterCollider = Player.GetComponent<Collider2D>();
    }

    void Update()
    {
        CheckPlayerInRange();
        if (!PM.isMoving)
            posssessed();
        canThroughtheWall();
        if (Player.transform.parent == null)
            Debug.Log("cnm");
        else
            Debug.Log(Player.transform.parent.name);
    }

    void CheckPlayerInRange()
    {
        // ʹ�� Physics2D.OverlapCircle ���Բ�η�Χ�ڵ�����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, playerLayer);

        if (colliders.Length > 0)
        {
            isPlayerInRange = true;
            // ���������ִ�������߼������翪ʼ׷����ҡ����ž�����Ч��
        }
        else
        {
            isPlayerInRange = false;
        }
    }

    // ���ӻ�Բ�η�Χ�����ڵ��ԣ�
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;  // ������ɫΪ��ɫ
        Gizmos.DrawWireSphere(transform.position, detectionRadius);  // ����һ��Բ�η�Χ
    }

    void posssessed()
    {
        if (isPlayerInRange && PM.enabled)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                possessedMove.enabled = true;
                IsPossessed = true;
                PM.isPossessed = true;
                SetTransparency(characterRenderer, targetAlpha);
                Player.transform.position = transform.position;
                Player.transform.SetParent(transform);
            }
        }
        if (IsPossessed)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                possessedMove.enabled = false;
                IsPossessed = false;
                PM.enabled = true;
                PM.isPossessed = false;
                SetTransparency(characterRenderer, 1f);
                Player.transform.SetParent(null);
            }
        }
    }

    void SetTransparency(Renderer renderer, float alpha)
    {
        // ��ȡ��ǰ���ʵ���ɫ
        Color currentColor = renderer.material.color;

        // �޸�Alphaͨ����͸���ȣ�
        currentColor.a = alpha;

        // Ӧ���µ���ɫ������
        renderer.material.color = currentColor;

        // ���ò���Ϊ͸��ģʽ
        renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        renderer.material.SetInt("_ZWrite", 0);
        renderer.material.DisableKeyword("_ALPHATEST_ON");
        renderer.material.EnableKeyword("_ALPHABLEND_ON");
        renderer.material.renderQueue = 3000;
    }

    void canThroughtheWall()
    {
        if (IsPossessed)
        {
            if (characterCollider != null)
            {
                characterCollider.isTrigger = false;
            }
        }
        else characterCollider.isTrigger = true;
    }
}
