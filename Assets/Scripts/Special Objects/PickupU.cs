using System.Collections;
using UnityEngine;

public class PickupU : MonoBehaviour
{
    public float detectionRadius;  // 圆形检测范围的半径
    public LayerMask dreamerLayer;       // 玩家所在的层（可以通过 Inspector 设置）
    public bool isspyInRange;
    //间谍与真正u盘发生碰撞，u盘就会消失（实际效果是被捡起）
    Renderer arenderer;

    private void Start()
    {
        arenderer = GetComponent<Renderer>();
        arenderer.enabled = false;
        isspyInRange = false;
        gameObject.SetActive(true);
    }
    private void Update()
    {
        CheckKeyInRange();
        StartCoroutine(Timer());
    }
    void CheckKeyInRange()
    {
        // 使用 Physics2D.OverlapCircle 检测圆形范围内的物体
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, dreamerLayer);

        if (colliders.Length > 0)
        {
            arenderer.enabled = true;
        }
        else
        {
            arenderer.enabled = false;
        }
    }

    IEnumerator Timer()
    {
        if (arenderer.enabled)
        {
            yield return new WaitForSeconds(0.1f);
            gameObject.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
            gameObject.SetActive(true);
        }

    }
}