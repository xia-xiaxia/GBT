using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CompletionUI : MonoBehaviour
{
    private RectTransform canvas;
    private RectTransform completionBg;
    private RectTransform text_1;
    private RectTransform text_2;
    private Button menuButton;



    public static CompletionUI Instance { get; private set; }

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
        canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        completionBg = transform.Find("UI/CompletionBg").GetComponent<RectTransform>();
        text_1 = transform.Find("UI/CompletionBg/Text_1").GetComponent<RectTransform>();
        text_2 = transform.Find("UI/CompletionBg/Text_2").GetComponent<RectTransform>();
        menuButton = transform.Find("UI/CompletionBg/MenuButton").GetComponent<Button>();
        completionBg.sizeDelta = canvas.sizeDelta;
        completionBg.anchoredPosition = Vector2.zero;
        text_1.anchoredPosition = new Vector2(-148, -97);
        text_2.anchoredPosition = new Vector2(163, -113);
        menuButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-127, 43);
        menuButton.onClick.AddListener(OnMenuButtonClicked);
    }
    public async void OnMenuButtonClicked()
    {
        transform.Find("UI").gameObject.SetActive(false);
        await TransitionManager_2.Instance.TransitionIn(1f, 5);
        await SceneManager.UnloadSceneAsync(GameManager.Instance.level);
        BgUI.Instance.ShowBg();
        StartUI.Instance.transform.Find("UI").gameObject.SetActive(true);
        await Task.Delay(1000);
        await TransitionManager_2.Instance.TransitionOut(5);
    }
}