using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Door doorController; // 引用 DoorController2D 脚本
    public Possessed possessed;
    public Key Key;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("dreamer"))  // 如果是梦主进入触发器区域
        {
            if (Key.getKey)
                doorController.ToggleDoor(); // 切换门的状态
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("dreamer"))  // 如果是玩家离开触发器区域
        {
            doorController.ToggleDoor(); // 切换门的状态
        }
    }
}
