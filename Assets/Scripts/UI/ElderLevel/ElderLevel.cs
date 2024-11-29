using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ElderLevel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler//接口用来检测点击
{
    [SerializeField] private Image image;
    [SerializeField] private Text nameText;

    public CanvasGroup canvasGroup;
    public int infoIndex;
    public int itemIndex;
    public RectTransform rectTransform;

    private bool isDrag;



    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void SetInfo(Sprite sprite, string name, int infoIndex)
    {
        image.sprite = sprite;
        nameText.text = name;
        this.infoIndex = infoIndex;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isDrag = false;
        ElderLevelScroll.Instance.OnPointerDown(eventData);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDrag)
        {
            ElderLevelScroll.Instance.Select(itemIndex, infoIndex, rectTransform);
        }
        ElderLevelScroll.Instance.OnPointerUp(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
        ElderLevelScroll.Instance.OnDrag(eventData);
    }
}