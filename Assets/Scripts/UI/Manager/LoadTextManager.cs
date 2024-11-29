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
    /// 传入指定文本框textBox，文本名name，继续按钮continueButton
    /// </summary>
    public void LoadText(TextMeshProUGUI textBox, string name, Button continueButton)
    {
        this.textBox = textBox;
        this.continueButton = continueButton;
        this.continueButton.onClick.AddListener(OnContinueBottonClicked);
        this.continueButton.transform.Find("ContinueText").GetComponent<TextMeshProUGUI>().text = "Continue";
        text = Texts.Find(t => t.name == name.Trim());
        text.index = 0;
        StartCoroutine(LoadOneByOne());
    }
    /// <summary>
    /// IsMouseButton is true when the text is loaded by clicking the mouse button.
    /// But in fact it is realized by clicking the continue button that is full-screen.
    /// </summary>
    public void LoadText(TextMeshProUGUI textBox, string name, Button continueButton, bool isMouseButton)
    {
        this.textBox = textBox;
        this.continueButton = continueButton;
        this.continueButton.onClick.AddListener(OnMouseButtonClicked);
        AText temp = Texts.Find(t => t.name == name.Trim());
        temp.index = 0;
        string tempContent = string.Join("\n", temp.content);
        text = new AText { name = temp.name, content = new string[] { tempContent }, index = temp.index };
        
        StartCoroutine(LoadOneByOne());
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
                yield return new WaitForSeconds(0.05f);
            }
        }
        textLoad = TextLoad.Loaded;
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
                        continueButton.transform.Find("ContinueText").GetComponent<TextMeshProUGUI>().text = "End";
                    text.index++;
                    StartCoroutine(LoadOneByOne());
                }
                else
                {
                    OnTextLoaded.Invoke();
                }
                break;
            default:
                break;
        }
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