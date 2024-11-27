using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailsUI : MonoBehaviour
{
    public static DetailsUI Instance {  get; private set; }

    private Button closeButton;

    private new TextMeshProUGUI name;
    private TextMeshProUGUI description;



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
        closeButton = transform.Find("UI/CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(CloseDetailsUI);
        name = transform.Find("UI/Name").GetComponent<TextMeshProUGUI>();
        description = transform.Find("UI/Description").GetComponent<TextMeshProUGUI>();
        transform.Find("UI").gameObject.SetActive(false);
    }
    public void ShowDetails(string name, AText text)
    {
        transform.Find("UI").gameObject.SetActive(true);
        this.name.text = name;

        string details = "";
        foreach (string content in text.content)
        {
            details += content + "\n";
        }
        description.text = details;
    }
    public void CloseDetailsUI()
    {
        transform.Find("UI").gameObject.SetActive(false);
        InteractableObjectManager.Instance.select = Select.Unselected;
    }
}
