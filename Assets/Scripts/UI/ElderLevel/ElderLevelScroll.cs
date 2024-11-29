using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ElderLevelScroll : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler//接口用来检测点击
{
    public static ElderLevelScroll Instance { get; private set; }

    public event Action<int> SelectAction;

    public GameObject levelPrefab;
    private RectTransform parent;

    private int displayNumber;//显示数量（奇数）
    private float levelSpace;//选项间隔
    private float moveSmooth;
    private float dragSpeed;
    private float scaleMultiplying;
    private float alphaMultiplying;
    private float displayWidth;
    private int offsetTimes;
    private int currentItemIndex;
    private float[] distances;
    private float selectItemX;

    private LevelInfo[] levelInfos;
    private ElderLevel[] levels;


    private bool isDrag;
    private bool isSelectMove;
    private bool isSelected;



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
        parent = transform.Find("UI").GetComponent<RectTransform>();
    }
    public void ShowLevel()
    {
        parent.gameObject.SetActive(true);
        Init();
        MoveLevel(0);
    }
    private void Init()
    {
        displayWidth = (displayNumber - 1) * levelSpace;
        levels = new ElderLevel[displayNumber];
        for (int i = 0; i < displayNumber; i++)
        {
            ElderLevel level = Instantiate(levelPrefab, parent).GetComponent<ElderLevel>();
            level.itemIndex = i;
            levels[i] = level;
        }
    }
    public void SetLevelInfo(string[] names, Sprite[] sprites)
    {
        //if (names.Length != sprites.Length || names.Length != descriptions.Length || names.Length != descriptions.Length)
        //{
        //    Debug.LogError("选择数据不完整");
        //    return;
        //}
        levelInfos = new LevelInfo[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            levelInfos[i] = new LevelInfo(names[i], sprites[i]);
        }
        SelectAction = null;
        isSelected = false;
    }
    public void Select(int itemIndex, int infoIndex, RectTransform itemRectTransform)
    {
        if (!isSelected && itemIndex == currentItemIndex)
        {
            SelectAction?.Invoke(infoIndex);
            isSelected = true;
            Debug.Log("Select" + (infoIndex + 1).ToString());
        }
        else
        {
            isSelected = true;
            selectItemX = itemRectTransform.localPosition.x;
        }
    }
    private void MoveLevel(int offsetTimes)
    {
        for (int i = 0; i < displayNumber; i++)
        {
            float x = levelSpace * (i - offsetTimes) - displayWidth / 2;
            levels[i].rectTransform.localPosition = new Vector2(x, levels[i].rectTransform.localPosition.y);
        }
        int middle;
        if (offsetTimes > 0)
        {
            middle = levelInfos.Length - offsetTimes % levelInfos.Length;
        }
        else
        {
            middle = -offsetTimes % levelInfos.Length;
        }
        int infoIndex = middle;
        for (int i = Mathf.FloorToInt(displayNumber / 2f); i < displayNumber; i++)
        {
            if (infoIndex >= levelInfos.Length)
            {
                infoIndex = 0;
            }
            levels[i].SetInfo(levelInfos[infoIndex].sprite, levelInfos[infoIndex].name, infoIndex);
            infoIndex++;
        }
        infoIndex = middle - 1;
        for (int i = Mathf.FloorToInt(displayNumber / 2f) - 1; i >= 0; i--)
        {
            if (infoIndex < 0)
            {
                infoIndex = levelInfos.Length - 1;
            }
            levels[i].SetInfo(levelInfos[infoIndex].sprite, levelInfos[infoIndex].name, infoIndex);
            infoIndex--;
        }
    }
    private void Update()
    {
        if (!isDrag)//没有拖拽则自动吸附
        {
            Adsorption();
        }
        int currentOffsetTimes = Mathf.FloorToInt(parent.anchoredPosition.x / levelSpace);
        if (currentOffsetTimes != offsetTimes)
        {
            offsetTimes = currentOffsetTimes;
            MoveLevel(offsetTimes);
        }
        ItemsControl();
    }
    private void ItemsControl()
    {
        distances = new float[displayNumber];
        for (int i = 0; i < displayNumber; i++)
        {
            float distance = Mathf.Abs(levels[i].rectTransform.position.x - transform.position.x);
            distances[i] = distance;
            float scale = 1 - distance * scaleMultiplying;
            levels[i].rectTransform.localScale = new Vector3(scale, scale, 1);
            levels[i].canvasGroup.alpha = 1 - distance * alphaMultiplying;
        }
        float minDistance = levelSpace * displayNumber;
        int minIndex = 0;
        for (int i = 0; i < displayNumber; i++)
        {
            if (distances[i] < minDistance)
            {
                minDistance = distances[i];
                minIndex = i;
            }
        }
        currentItemIndex = levels[minIndex].itemIndex;
    }
    private void Adsorption()
    {
        float targetX;
        if (!isSelectMove)
        {
            float distance = parent.localPosition.x % levelSpace;
            int times = Mathf.FloorToInt(parent.localPosition.x / levelSpace);
            if (distance > 0)
            {
                if (distance < levelSpace / 2)
                {
                    targetX = times * levelSpace;
                }
                else
                {
                    targetX = (times + 1) * levelSpace;
                }
            }
            else
            {
                if (distance < -levelSpace / 2)
                {
                    targetX = times * levelSpace;
                }
                else
                {
                    targetX = (times + 1) * levelSpace;
                }
            }
        }
        else
        {
            targetX = -selectItemX;
        }
        parent.localPosition = new Vector2(Mathf.Lerp(parent.localPosition.x, targetX, moveSmooth / 10), parent.localPosition.y);
    }
    public void OnDrag(PointerEventData eventData)
    {
        isSelectMove = false;
        parent.anchoredPosition = new Vector2(parent.localPosition.x + eventData.delta.x * dragSpeed, parent.localPosition.y);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isDrag = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isDrag = false;
    }
}

public class LevelInfo
{
    public string name;
    public Sprite sprite;
    public LevelInfo(string name, Sprite sprite)
    {
        this.name = name;
        this.sprite = sprite;
    }
}