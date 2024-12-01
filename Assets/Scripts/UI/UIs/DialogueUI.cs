using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    private GameObject dialogueBox;
    private GameObject dialogistBox;
    private GameObject continueButton;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;

        dialogistBox = transform.Find("UI/DialogistBox").gameObject;
        dialogueBox = transform.Find("UI/DialogueBox").gameObject;
        continueButton = transform.Find("UI/DialogueBox/ContinueButton").gameObject;
    }
    private void Start()
    {
        RectTransform canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        transform.Find("UI").GetComponent<RectTransform>().sizeDelta = canvas.sizeDelta;
        transform.Find("UI").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        dialogistBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 175);
        dialogueBox.GetComponent<RectTransform>().sizeDelta = new Vector2(canvas.sizeDelta.x - 100, 150);
        dialogueBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 80);
        continueButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-90, 30);
    }
    /// <summary>
    /// 传入对话者名字，如果对话者是人物，则true，否则为false，用以控制是否显示对话者名字,isAuto控制是否自动播放
    /// </summary>
    public void ShowDialogue(string name, bool isCharacter, bool isAuto)
    {
        transform.Find("UI").gameObject.SetActive(true);
        if (isCharacter)
        {
            dialogistBox.SetActive(true);
            dialogistBox.transform.Find("DialogistName").GetComponent<TextMeshProUGUI>().text = name;
        }
        else
        {
            dialogistBox.SetActive(false);
        }
        dialogueBox.SetActive(true);
        LoadTextManager.Instance.OnTextLoaded.AddListener(OnTextLoaded);
        if (isAuto)
        {
            continueButton.SetActive(false);
            LoadTextManager.Instance.LoadText(dialogueBox.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>(), name);
        }
        else
        {
            continueButton.SetActive(true);
            LoadTextManager.Instance.LoadText(dialogueBox.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>(), name, continueButton.GetComponent<Button>());
        }
    }
    private void OnTextLoaded()
    {
        LoadTextManager.Instance.OnTextLoaded.RemoveListener(OnTextLoaded);
        transform.Find("UI").gameObject.SetActive(false);
    }
}