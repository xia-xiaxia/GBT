using UnityEngine;

public class TriggerDetector : MonoBehaviour
{

    public float detectionRadius = 0.2f;  // 圆形检测范围的半径
    public LayerMask spyLayer;      
    public bool isspyInRange = false;
    //间谍与真正u盘发生碰撞，u盘就会消失（实际效果是被捡起）

    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void Update()
    {

        CheckKeyInRange();

    }


    void CheckKeyInRange()
    {
        // 使用 Physics2D.OverlapCircle 检测圆形范围内的物体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, spyLayer);

        if (colliders.Length > 0)
        {
            gameObject.SetActive(true);

        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
