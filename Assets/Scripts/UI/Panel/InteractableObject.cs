using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public new string name;
    private AText text;
    private Button selectButton;
    public GameObject correspondingObj;
    private GameObject state;



    private void Start()
    {
        name = gameObject.name;
        state = transform.Find("State").gameObject;
        selectButton = GetComponent<Button>();
        selectButton.onClick.AddListener(() => InteractableObjectManager.Instance.OnSelect(gameObject, Select.CompletelySelected));
    }
    public void OnSelected()
    {
        text = LoadTextManager.Instance.Texts.Find(t => t.name.Equals(name.Trim()));
        if (text == null)
        {
            Debug.LogError("No text found for " + name);
            return;
        }
        DetailsUI.Instance.ShowDetails(name, text);
    }
    public void OnTaged(InterObjState state)
    {
        switch (state)
        {
            case InterObjState.None:
                this.state.SetActive(false);
                this.state.GetComponent<Image>().color = Color.white;
                break;
            case InterObjState.Marked:
                this.state.SetActive(true);
                this.state.GetComponent<Image>().color = Color.red;
                break;
            case InterObjState.Possessed:
                this.state.SetActive(false);
                this.state.GetComponent<Image>().color = Color.green;
                break;
        }
    }
}
public enum InterObjState
{
    None,
    Marked,
    Possessed
}