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
        continueButton = transform.Find("UI/ContinueButton").gameObject;
    }
    /// <summary>
    /// ����Ի������֣�����Ի����������true������Ϊfalse�����Կ����Ƿ���ʾ�Ի�������,isAuto�����Ƿ��Զ�����
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