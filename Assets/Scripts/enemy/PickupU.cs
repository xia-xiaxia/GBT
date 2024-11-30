using UnityEngine;

public class PickupU: MonoBehaviour
{
  //间谍与真正u盘发生碰撞，u盘就会消失（实际效果是被捡起）
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision Detected with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("u"))
        {
            
            Destroy(collision.gameObject);//销毁u盘
            Debug.Log("Destroying object: " + collision.gameObject.name); // 打印日志以确认销毁
        }
    }

   
}

