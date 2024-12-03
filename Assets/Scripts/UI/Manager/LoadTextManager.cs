using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static LoadTextManager;

public class LoadTextManager : MonoBehaviour
{
    public static LoadTextManager Instance { get; private set; }
    public UnityEvent OnTextLoaded = new UnityEvent();

    public TextAsset textFile;

    public List<AText> Texts = new List<AText>();
    private AText text;
    private TextLoad textLoad;
    private TextMeshProUGUI textBox;
    private Button continueButton;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        string[] allText = textFile.text.Split(new string[] { "\r\n\r\n" }, System.StringSplitOptions.None);
        foreach (string eachText in allText)
        {
            if (!string.IsNullOrEmpty(eachText))
            {
                string[] nameAndTexts = eachText.Split(new string[] { ":\r\n" }, System.StringSplitOptions.None);
                string name = nameAndTexts[0].Trim();
                List<string> texts = new List<string>();
                foreach (string text in nameAndTexts[1].Split(new string[] { "\r\n" }, System.StringSplitOptions.None))
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        texts.Add(text);
                    }
                }
                Texts.Add(new AText { name = name, content = texts.ToArray(), index = 0 });
            }
        }
    }
    /// <summary>
    /// 点击继续按钮以继续
    /// </summary>
    public void LoadText(TextMeshProUGUI textBox, string name, Button continueButton)
    {
        this.textBox = textBox;
        this.continueButton = continueButton;
        this.continueButton.onClick.AddListener(OnContinueBottonClicked);
        this.continueButton.transform.Find("ContinueText").GetComponent<TextMeshProUGUI>().text = "继续";
        text = Texts.Find(t => t.name == name.Trim());
        text.index = 0;

        textLoad = TextLoad.Loading;
        StartCoroutine(LoadOneByOne());
    }
    /// <summary>
    /// 点击屏幕以继续
    /// </summary>
    public void LoadText(TextMeshProUGUI textBox, string name, Button continueButton, bool isMouseButton)
    {
        this.textBox = textBox;
        this.continueButton = continueButton;
        this.continueButton.onClick.AddListener(OnMouseButtonClicked);
        AText temp = Texts.Find(t => t.name == name);
        temp.index = 0;
        string tempContent = string.Join("\n", temp.content);
        text = new AText { name = temp.name, content = new string[] { tempContent }, index = temp.index };

        textLoad = TextLoad.Loading;
        StartCoroutine(LoadOneByOne());
    }
    /// <summary>
    /// 自动加载文本
    /// </summary>
    public void LoadText(TextMeshProUGUI textBox, string name)
    {
        this.textBox = textBox;
        text = Texts.Find(t => t.name == name.Trim());
        text.index = 0;

        StartCoroutine(AutoLoadOneByOne());
    }
    private IEnumerator LoadOneByOne()
    {
        int i = 0;
        while (i++ < text.content[text.index].Length && textLoad == TextLoad.Loading)
        {
            if (text.content[text.index][i - 1] == ' ')
                i++;
            else
            {
                textBox.text = text.content[text.index].Substring(0, i);
                yield return new WaitForSeconds(0.03f);
            }
        }
        textLoad = TextLoad.Loaded;
    }
    private IEnumerator AutoLoadOneByOne()
    {
        while (text.index < text.content.Length)
        {
            textLoad = TextLoad.Loading;
            StartCoroutine(LoadOneByOne());
            yield return new WaitUntil(() => textLoad == TextLoad.Loaded);
            yield return new WaitForSeconds(CalculateWaitTime());
            text.index++;
        }
    }
    private void OnMouseButtonClicked()
    {
        switch (textLoad)
        {
            case TextLoad.Loading:
                textLoad = TextLoad.Loaded;
                StopCoroutine(LoadOneByOne());
                textBox.text = text.content[text.index];
                break;
            case TextLoad.Loaded:
                continueButton.onClick.RemoveAllListeners();
                OnTextLoaded.Invoke();
                break;
            default:
                break;
        }
    }
    private void OnContinueBottonClicked()
    {
        switch (textLoad)
        {
            case TextLoad.Loading:
                textLoad = TextLoad.Loaded;
                StopCoroutine(LoadOneByOne());
                textBox.text = text.content[text.index];
                break;
            case TextLoad.Loaded:
                textLoad = TextLoad.Loading;
                if (text.index < text.content.Length - 1)
                {
                    if (text.index == text.content.Length - 2)
                        continueButton.transform.Find("ContinueText").GetComponent<TextMeshProUGUI>().text = "结束";
                    text.index++;
                    StartCoroutine(LoadOneByOne());
                }
                else
                {
                    continueButton.onClick.RemoveAllListeners();
                    OnTextLoaded.Invoke();
                }
                break;
            default:
                break;
        }
    }

    private float CalculateWaitTime()
    {
        int len = text.content[text.index].Length;
        if (len < 33)
            return 0.5f + 0.03f * len;
        else
            return 1.5f;
    }
}
public class AText
{
    public string name;
    public string[] content;
    public int index;
}
public enum TextLoad
{
    Loading,
    Loaded
}