using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public static TutorialUI Instance { get; private set; }

    public TrickExampleImage trickExampleImage;

    private Image exampleImage;
    private TextMeshProUGUI trickName;
    private TextMeshProUGUI tutorialContent;



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
        exampleImage = transform.Find("UI/ExampleImage").GetComponent<Image>();
        trickName = transform.Find("UI/TrickName").GetComponent<TextMeshProUGUI>();
        tutorialContent = transform.Find("UI/TutorialContent").GetComponent<TextMeshProUGUI>();
    }
    public void ShowTutorial(string name)
    {
        AText text = LoadTextManager.Instance.Texts.Find(t => t.name == name);
        Sprite sprite = trickExampleImage.sprites.Find(s => s.name == name+"_0");
        trickName.text = name;
        tutorialContent.text = "";
        foreach (string content in text.content)
        {
            tutorialContent.text += content;
        }
        exampleImage.sprite = sprite;
    }
}
