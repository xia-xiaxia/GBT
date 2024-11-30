using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgUI : MonoBehaviour
{
    public static BgUI Instance { get; private set; }



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    private void Start()
    {
        GetComponent<RectTransform>().sizeDelta = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta;
        GetComponent<RectTransform>().position = GameObject.Find("Canvas").GetComponent<RectTransform>().position;
    }
    public void ShowBg()
    {
        GetComponent<CanvasGroup>().alpha = 1;
    }
    public void HideBg()
    {
        GetComponent<CanvasGroup>().alpha = 0;
    }
}
