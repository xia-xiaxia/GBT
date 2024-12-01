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
    public Material material;

    private List<InteractableObject> interactableObjects;
    private GameObject selectedObject;
    [HideInInspector]
    public Select select;
    private string[] tags =
    {
        "Interactable",
        "Key",
        "Folder"
    };
    public Color emissionColor;
    public float emissionIntensity;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    private void OnEnable()
    {
        selectedObject = null;
        interactableObjects = new List<InteractableObject>();
        select = Select.Unselected;
    }
    private void Start()
    {
        graphicRaycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
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
                    Material mat = selectedObject.GetComponent<InteractableObject>().correspondingObj.GetComponent<SpriteRenderer>().material;
                    mat.DisableKeyword("_EMISSION");
                    mat.SetColor("_EmissionColor", Color.black);
                    selectedObject = null;
                }
                DetailsUI.Instance.transform.Find("UI").gameObject.SetActive(false);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (IsMouseOverUIObjectWithTag(Tag.DETAILSUI) == null)
            {
                if (IsMouseOverUIObjectWithTag(Tag.INTERACTABLE) == null)
                    DetailsUI.Instance.CloseDetailsUI();
            }
        }
    }
    public void LoadInteractableObjects()
    {
        List<GameObject> allInteractableObjects = new List<GameObject>();
        foreach (string tag in tags)
        {
            allInteractableObjects.AddRange(GameObject.FindGameObjectsWithTag(tag));
        }
        foreach (GameObject obj in allInteractableObjects)
        {
            GameObject interactableObject = Instantiate(ioPrefab, transform.Find("Viewport/Content"));
            interactableObject.name = obj.name;
            interactableObject.tag = Tag.INTERACTABLE;
            if (obj.GetComponent<Possessed>() != null)
                obj.GetComponent<Possessed>().correspondingUIObj = interactableObject;
            else if (obj.GetComponent<Metamorphosm>() != null)
                obj.GetComponent<Metamorphosm>().correspondingUIObj = interactableObject;
            interactableObject.GetComponent<InteractableObject>().correspondingObj = obj;
            interactableObject.transform.Find("Index").GetComponent<TextMeshProUGUI>().text = (interactableObjects.Count + 1).ToString();
            interactableObject.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = obj.name;
            interactableObject.transform.Find("Sprite").GetComponent<Image>().sprite = obj.GetComponent<SpriteRenderer>().sprite;
            interactableObject.GetComponent<InteractableObject>().correspondingObj.GetComponent<SpriteRenderer>().material = material;
            interactableObjects.Add(interactableObject.GetComponent<InteractableObject>());
        }
    }
    /// <summary>
    /// 调用逻辑：每个InteractableObject的Button组件的OnClick事件||InteractableObjectManager的Update检测
    /// --传入被选择的物体-->InteractableObjectManager的OnSelect方法，处理面板上物体的显示
    /// ---->选中物体的OnSelected方法，显示详情页面
    /// </summary>
    /// <param name="selectedObj">被选择的物体</param >
    public void OnSelect(GameObject selectedObj, Select s)
    {
        Material mat;
        if (selectedObject != null)
        {
            selectedObject.transform.Find("Halo").gameObject.SetActive(false);
            mat = selectedObject.GetComponent<InteractableObject>().correspondingObj.GetComponent<SpriteRenderer>().material;
            mat.DisableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", Color.black);
        }
        select = s;
        selectedObject = selectedObj;
        selectedObject.transform.Find("Halo").gameObject.SetActive(true);
        mat = selectedObject.GetComponent<InteractableObject>().correspondingObj.GetComponent<SpriteRenderer>().material;
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", emissionColor * emissionIntensity);
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