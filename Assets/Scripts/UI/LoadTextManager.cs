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

    private List<AText> Texts = new List<AText>();
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
    }
    private void Start()//读取文本
    {
        string[] allText = textFile.text.Split("\r\n\r\n");
        foreach (string eachText in allText)
        {
            if (!string.IsNullOrEmpty(eachText))
            {
                string[] nameAndTexts = eachText.Split(":\r\n");
                string name = nameAndTexts[0].Trim();
                List<string> texts = new List<string>();
                foreach (string text in nameAndTexts[1].Split("\r\n"))
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
    public void LoadText(TextMeshProUGUI textBox, string name, Button continueButton)
    {
        //包含：根据name找到文本，在传入的对话框中逐字载入文本，用continueButton控制载入速度
        this.textBox = textBox;
        this.continueButton = continueButton;
        continueButton.onClick.AddListener(OnContinueBottonClicked);
        continueButton.transform.Find("ContinueText").GetComponent<TextMeshProUGUI>().text = "Continue";
        text = Texts.Find(t => t.name == name);
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