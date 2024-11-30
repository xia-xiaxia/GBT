using UnityEngine;

public class PickupU: MonoBehaviour
{
    public float detectionRadius = 0.2f;  // 圆形检测范围的半径
    public LayerMask dreamerLayer;       // 玩家所在的层（可以通过 Inspector 设置）
    public bool isspyInRange = false;
    //间谍与真正u盘发生碰撞，u盘就会消失（实际效果是被捡起）

    private void Start()
    {
        gameObject.SetActive(true);
    }
    private void Update()
    {

        CheckKeyInRange();
    
    }


    void CheckKeyInRange()
    {
        // 使用 Physics2D.OverlapCircle 检测圆形范围内的物体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, dreamerLayer);

        if (colliders.Length > 0)
        {
            gameObject.SetActive(false);

        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}

