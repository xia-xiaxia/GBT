using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public new string name;
    private AText text;
    private Button selectButton;
    public GameObject correspondingObj;



    private void Start()
    {
        name = gameObject.name;
        selectButton = GetComponent<Button>();
        selectButton.onClick.AddListener(() => InteractableObjectManager.Instance.OnSelect(gameObject, Select.CompletelySelected));
    }
    public void OnSelected()
    {
        text = LoadTextManager.Instance.Texts.Find(t => t.name.Equals(name.Trim()));
        if (text == null)
        {
            Debug.LogError("No text found for " + name + " The length is " + name.Length);
            return;
        }
        DetailsUI.Instance.ShowDetails(name, text);
    }
}
