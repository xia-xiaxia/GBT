using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TODO : MonoBehaviour
{
    //文本注意中文还是英文冒号
    //物品堆叠
    //字典代替AText
    //加载不下的字可以滚动显示

    //UI大小适配不同屏幕大小：转场，对话框，功能板
    //UI动画

    //幕中：剧情播放间的转场
    //Help键：显示操作说明
    //氧气条
    //剧情与睁闭眼

    //异步加载场景(或者用空场景做转场)(还有加载条)
    //单独UI场景

    //关卡调度

    /*1.	每个关卡场景都有一个UI物体：
    优点：
    简单直接，每个关卡场景可以有独立的UI，便于管理和修改。
    UI与关卡场景紧密结合，适合需要在不同关卡中显示不同UI的情况。
    缺点：
    可能会导致重复的UI元素，增加维护成本。
    切换场景时，UI也会重新加载，可能影响性能。
    2.	将UI放在一个单独的场景中，需要时显示：
    优点：
    UI可以在多个关卡之间共享，减少重复代码和资源。
    切换关卡时，UI不会重新加载，提高性能。
    便于集中管理和更新UI元素。
    缺点：
    需要额外的逻辑来管理UI的显示和隐藏。
    可能需要处理UI与关卡场景之间的交互。
    在实际开发中，第二种方法（将UI放在一个单独的场景中）通常更为常见，尤其是在需要共享UI元素（如主菜单、暂停菜单、HUD等）时。以下是一个示例，展示如何在Unity中实现这种方法：
    1.	创建一个单独的UI场景：
    创建一个新的场景，命名为 UIScene。
    在 UIScene 中添加所有需要的UI元素（如主菜单、暂停菜单、HUD等）。
    2.	在主场景中加载UI场景：
    在主场景的脚本中，使用 SceneManager.LoadSceneAsync 方法加载 UIScene，并设置 LoadSceneMode.Additive 参数。
    using UnityEngine;
    using UnityEngine.SceneManagement;


    public class GameManager : MonoBehaviour
    {
        private void Start()
        {
            // 加载UI场景
            SceneManager.LoadSceneAsync("UIScene", LoadSceneMode.Additive);
        }
    }
    3.管理UI的显示和隐藏：
    在 UIScene 中的脚本中，添加逻辑来管理UI的显示和隐藏。例如，可以使用单例模式来控制UI的显示状态。
    示例代码：

    using UnityEngine;

    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            Instance = this;
            DontDestroyOnLoad(gameObject); // 确保UI场景在切换关卡时不会被销毁
        }

        public void ShowUI()
        {
            // 显示UI
            gameObject.SetActive(true);
        }

        public void HideUI()
        {
            // 隐藏UI
            gameObject.SetActive(false);
        }
    }
    通过这种方法，你可以在多个关卡之间共享UI，提高性能并减少重复代码。
    */
}