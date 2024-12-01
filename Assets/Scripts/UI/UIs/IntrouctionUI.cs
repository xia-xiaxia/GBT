using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntrouctionUI : MonoBehaviour
{
    public static IntrouctionUI Instance { get; private set; }

    private TextMeshProUGUI text;
    private Button continueButton;



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
        text = transform.Find("UI/Text").GetComponent<TextMeshProUGUI>();
        continueButton = transform.Find("UI/Continue").GetComponent<Button>();
        continueButton.GetComponent<RectTransform>().sizeDelta = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta;
        transform.Find("UI").gameObject.SetActive(false);
    }
    public void ShowIntroduction(string level)
    {
        transform.Find("UI").gameObject.SetActive(true);
        LoadTextManager.Instance.OnTextLoaded.AddListener(OnTextLoaded);
        LoadTextManager.Instance.LoadText(text, level + "����", continueButton, true);
    }
    private void OnTextLoaded()
    {
        LoadTextManager.Instance.OnTextLoaded.RemoveListener(OnTextLoaded);
        transform.Find("UI").gameObject.SetActive(false);
    }
}
