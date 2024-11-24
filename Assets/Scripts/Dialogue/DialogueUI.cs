using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }

    public TextAsset dialogueFile;
    private GameObject dialogueBox;
    private GameObject dialogistBox;
    private GameObject continueButton;

    private List<Dialogue> dialogues = new List<Dialogue>();
    private Dialogue dialogue;
    private DialogueLoad dialogueLoad;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;

        dialogueBox = transform.Find("UI/DialogueBox").gameObject;
        dialogistBox = transform.Find("UI/DialogistBox").gameObject;
        continueButton = transform.Find("UI/ContinueButton").gameObject;
    }
    private void Start()
    {
        string[] allDialogue = dialogueFile.text.Split("\r\n\r\n");
        foreach (string eachDialogue in allDialogue)
        {
            if (!string.IsNullOrEmpty(eachDialogue))
            {
                string[] nameAndTexts = eachDialogue.Split(":\r\n");
                string name = nameAndTexts[0].Trim();
                List<string> texts = new List<string>();
                foreach (string text in nameAndTexts[1].Split("\r\n"))
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        texts.Add(text);
                    }
                }
                dialogues.Add(new Dialogue { name = name, texts = texts.ToArray(), index = 0 });
            }
        }
        LoadDialogue(dialogues[0].name, true);
    }
    /// <summary>
    /// 传入对话者名字，如果对话者是人物，则true，否则为false，用以控制是否显示对话者名字
    /// </summary>
    public void LoadDialogue(string name, bool isCharacter)
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
        dialogue = dialogues.Find(d => d.name == name);
        dialogue.index = 0;
        continueButton.transform.Find("ContinueText").GetComponent<TextMeshProUGUI>().text = "Continue";
        if (dialogue != null)
        {
            dialogueBox.SetActive(true);
            dialogueLoad = DialogueLoad.Loading;
            StartCoroutine(LoadOneByOne());
        }
    }
    public IEnumerator LoadOneByOne()
    {
        int i = 0;
        while (i++ < dialogue.texts[dialogue.index].Length && dialogueLoad == DialogueLoad.Loading)
        {
            if (dialogue.texts[dialogue.index][i - 1] == ' ')
                i++;
            else
            {
                dialogueBox.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>().text = dialogue.texts[dialogue.index].Substring(0, i);
                yield return new WaitForSeconds(0.05f);
            }
        }
        dialogueLoad = DialogueLoad.Loaded;
    }
    public void OnContinueBottonClicked()
    {
        switch (dialogueLoad)
        {
            case DialogueLoad.Loading:
                dialogueLoad = DialogueLoad.Loaded;
                StopCoroutine(LoadOneByOne());
                dialogueBox.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>().text = dialogue.texts[dialogue.index];
                break;
            case DialogueLoad.Loaded:
                dialogueLoad = DialogueLoad.Loading;
                if (dialogue.index < dialogue.texts.Length - 1)
                {

                    if (dialogue.index == dialogue.texts.Length - 1)
                        continueButton.transform.Find("ContinueText").GetComponent<TextMeshProUGUI>().text = "End";
                    dialogue.index++;
                    StartCoroutine(LoadOneByOne());
                }
                else
                {
                    transform.Find("UI").gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }
    }
}
public class Dialogue
{
    public string name;
    public string[] texts;
    public int index;
}
public enum DialogueLoad
{
    Loading,
    Loaded
}