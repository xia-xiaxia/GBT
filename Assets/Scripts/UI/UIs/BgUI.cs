using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgUI : MonoBehaviour
{
    public static BgUI Instance { get; private set; }
    public Button bgButton;
    private Button backButton;



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
        RectTransform canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        transform.Find("UI/Image").GetComponent<RectTransform>().sizeDelta = canvas.sizeDelta;
        transform.Find("UI/Image").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        backButton = transform.Find("UI/BackButton").GetComponent<Button>();
        backButton.onClick.AddListener(() => { transform.Find("UI").gameObject.SetActive(false); });
        bgButton.onClick.AddListener(() => { transform.Find("UI").gameObject.SetActive(true); });
    }
}
