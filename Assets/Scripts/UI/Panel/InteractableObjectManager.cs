using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractableObjectManager : MonoBehaviour
{
    public static InteractableObjectManager Instance { get; private set; }

    public GameObject ioPrefab;
    private GraphicRaycaster graphicRaycaster;
    private EventSystem eventSystem;

    private List<InteractableObject> interactableObjects = new List<InteractableObject>();
    private GameObject selectedObject = null;
    [HideInInspector]
    public Select select = Select.Unselected;



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
        graphicRaycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        LoadInteractableObjects();
    }
    private void Update()
    {
        if (select == Select.Unselected || select == Select.Selected)
        {
            GameObject go = IsMouseOverUIObjectWithTag(Tag.INTERACTABLE);
            if (go != null)
            {
                OnSelect(go, Select.Selected);
            }
            else
            {
                select = Select.Unselected;
                if (selectedObject != null)
                {
                    selectedObject.transform.Find("Halo").gameObject.SetActive(false);
                    selectedObject = null;
                }
                DetailsManager.Instance.transform.Find("UI").gameObject.SetActive(false);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            GameObject go = IsMouseOverUIObjectWithTag(Tag.DETAILSUI);
            if (go == null)
            {
                DetailsManager.Instance.CloseDetailsUI();
            }
        }
    }
    public void LoadInteractableObjects()
    {
        List<GameObject> allInteractableObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag(Tag.INTERACTABLE));
        foreach (GameObject obj in allInteractableObjects)
        {
            GameObject interactableObject = Instantiate(ioPrefab, transform.Find("Viewport/Content"));
            interactableObject.name = obj.name;
            interactableObject.tag = Tag.INTERACTABLE;
            interactableObject.transform.Find("Index").GetComponent<TextMeshProUGUI>().text = (interactableObjects.Count + 1).ToString();
            interactableObject.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = obj.name;
            interactableObject.transform.Find("Sprite").GetComponent<Image>().sprite = obj.GetComponent<SpriteRenderer>().sprite;
            interactableObjects.Add(interactableObject.GetComponent<InteractableObject>());
        }
    }
    /// <summary>
    /// �����߼���ÿ��InteractableObject��Button�����OnClick�¼�||InteractableObjectManager��Update���
    /// --���뱻ѡ�������-->InteractableObjectManager��OnSelect����������������������ʾ
    /// ---->ѡ�������OnSelected��������ʾ����ҳ��
    /// </summary>
    /// <param name="selectedObj">��ѡ�������</param >
    public void OnSelect(GameObject selectedObj, Select s)
    {
        if (selectedObject != null)
        {
            selectedObject.transform.Find("Halo").gameObject.SetActive(false);
        }
        select = s;
        selectedObject = selectedObj;
        selectedObject.transform.Find("Halo").gameObject.SetActive(true);
        selectedObject.GetComponent<InteractableObject>().OnSelected();
    }
    private GameObject IsMouseOverUIObjectWithTag(string tag)
    {
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, results);

        if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag(tag))
                {
                    return result.gameObject;
                }
            }
        }
        return null;
    }
}
public enum Select
{
    Unselected,
    Selected,
    CompletelySelected
}