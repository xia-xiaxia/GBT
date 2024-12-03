using System.Collections;
using UnityEngine;

public class PickupUUU : MonoBehaviour
{
    public float detectionRadius;  // 圆形检测范围的半径
    public LayerMask dreamerLayer;       // 玩家所在的层（可以通过 Inspector 设置）
    public bool isspyInRange;

    public GameObject U;
    private Renderer arenderer;


    private void Start()
    {
        if (U == null)
        {
           //Debug.LogError("U 未赋值，请在 Inspector 中指定一个有效的 GameObject！");
            return; // 如果 U 为空，就不执行下面的代码
        }
        arenderer = U.GetComponent<Renderer>();
        isspyInRange = false;
        arenderer.enabled = false;
    }
    private void Update()
    {
        
            if (U == null)
            {
                //Debug.LogError("U 未赋值，请在 Inspector 中指定一个有效的 GameObject！");
                return; // 如果 U 为空，就不执行下面的代码
            }
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
            U.SetActive(false);
        }
     
    }

}