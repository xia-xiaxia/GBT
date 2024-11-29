using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public ElderLevelScroll horizontalScroll;

    private void Awake()
    {
        if (horizontalScroll != null)
        {
            int num = 100;
            string[] names = new string[num];
            Sprite[] sprites = new Sprite[num];
            for (int i = 0; i < num; i++)
            {
                names[i] = (i + 1).ToString();
                sprites[i] = null;
            }
            horizontalScroll.SetLevelInfo(names, sprites);
            horizontalScroll.SelectAction += (index) =>
            {
                Debug.Log("SelectAction:" + index);
            };
        }
    }
}
