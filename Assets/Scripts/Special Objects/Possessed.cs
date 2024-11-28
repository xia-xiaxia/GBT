using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possessed : MonoBehaviour
{
    public PlayerMovement PM;
    private PossessedMove PossessedMove;
    public float detectionRadius = 5f;  // 圆形检测范围的半径
    public LayerMask playerLayer;       // 玩家所在的层（可以通过 Inspector 设置）
    public bool isPlayerInRange = false; // 玩家是否在检测范围内
    public GameObject Player;
    public float targetAlpha = 0.5f; // 目标透明度
    private Renderer characterRenderer;
    private Collider2D characterCollider;

    public bool IsPossessed;


    void Start()
    {
        IsPossessed = false;
        PossessedMove = GetComponent<PossessedMove>();
        PossessedMove.enabled = false;
        characterRenderer = Player.GetComponent<Renderer>();
        characterCollider = Player.GetComponent<Collider2D>();
    }

    void Update()
    {
        CheckPlayerInRange();
        if(!PM.isMoving)
            posssessed();
        PM.isPossessed = IsPossessed;
        canThroughtheWall();
    }

    void CheckPlayerInRange()
    {
        // 使用 Physics2D.OverlapCircle 检测圆形范围内的物体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, playerLayer);

        if (colliders.Length > 0)
        {
            isPlayerInRange = true;
            // 在这里可以执行其他逻辑，例如开始追踪玩家、播放警告音效等
        }
        else
        {
            isPlayerInRange = false;
        }
    }

    // 可视化圆形范围（用于调试）
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;  // 设置颜色为绿色
        Gizmos.DrawWireSphere(transform.position, detectionRadius);  // 绘制一个圆形范围
    }

    void posssessed()
    {
        if (isPlayerInRange && PM.enabled)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                PossessedMove.enabled = true;
                PM.enabled = false;
                IsPossessed = true;
                SetTransparency(characterRenderer, targetAlpha);
                Player.transform.position = transform.position;
                Player.transform.SetParent(transform, true);
            }
        }
        if (PM.enabled == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                IsPossessed = false;
                PossessedMove.enabled = false;
                PM.enabled = true;
                SetTransparency(characterRenderer, 1f);
                Player.transform.SetParent(transform, false);
            }
        }
    }

    void SetTransparency(Renderer renderer, float alpha)
    {
        // 获取当前材质的颜色
        Color currentColor = renderer.material.color;

        // 修改Alpha通道（透明度）
        currentColor.a = alpha;

        // 应用新的颜色到材质
        renderer.material.color = currentColor;

        // 设置材质为透明模式
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
