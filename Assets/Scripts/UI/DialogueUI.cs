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

        dialogueBox = transform.Find("UI/DialogueBox").gameObject;
        dialogistBox = transform.Find("UI/DialogistBox").gameObject;
        continueButton = transform.Find("UI/ContinueButton").gameObject;
    }
    private void Start()
    {
        LoadDialogue("Sam", true);
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
        dialogueBox.SetActive(true);
        LoadTextManager.Instance.OnTextLoaded.AddListener(OnTextLoaded);
        LoadTextManager.Instance.LoadText(dialogueBox.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>(), name, continueButton.GetComponent<Button>());
    }
    private void OnTextLoaded()
    {
        transform.Find("UI").gameObject.SetActive(false);
        LoadTextManager.Instance.OnTextLoaded.RemoveListener(OnTextLoaded);
    }
}
//    public IEnumerator LoadOneByOne()
//    {
//        int i = 0;
//        while (i++ < dialogue.texts[dialogue.index].Length && dialogueLoad == DialogueLoad.Loading)
//        {
//            if (dialogue.texts[dialogue.index][i - 1] == ' ')
//                i++;
//            else
//            {
//                dialogueBox.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>().text = dialogue.texts[dialogue.index].Substring(0, i);
//                yield return new WaitForSeconds(0.05f);
//            }
//        }
//        dialogueLoad = DialogueLoad.Loaded;
//    }
//    public void OnContinueBottonClicked()
//    {
//        switch (dialogueLoad)
//        {
//            case DialogueLoad.Loading:
//                dialogueLoad = DialogueLoad.Loaded;
//                StopCoroutine(LoadOneByOne());
//                dialogueBox.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>().text = dialogue.texts[dialogue.index];
//                break;
//            case DialogueLoad.Loaded:
//                dialogueLoad = DialogueLoad.Loading;
//                if (dialogue.index < dialogue.texts.Length - 1)
//                {
//                    if (dialogue.index == dialogue.texts.Length - 1)
//                        continueButton.transform.Find("ContinueText").GetComponent<TextMeshProUGUI>().text = "End";
//                    dialogue.index++;
//                    StartCoroutine(LoadOneByOne());
//                }
//                else
//                {
//                    transform.Find("UI").gameObject.SetActive(false);
//                }
//                break;
//            default:
//                break;
//        }
//    }
//public class Dialogue
//{
//    public string name;
//    public string[] texts;
//    public int index;
//}
//public enum DialogueLoad
//{
//    Loading,
//    Loaded
//}