using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public static LevelUI Instance { get; private set; }

    public LevelDatabase levelDatabase;
    public List<GameObject> levelList;
    private Button leftButton;
    private Button rightButton;
    private Button chooseButton;
    public TextMeshProUGUI levelName;

    private List<Vector3> positions = new List<Vector3>();
    private List<float> alphas = new List<float>();
    private List<Vector2> sizeDelta = new List<Vector2>();
    public int curLevelIndex;



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
        leftButton = transform.Find("UI/LeftButton").GetComponent<Button>();
        rightButton = transform.Find("UI/RightButton").GetComponent<Button>();
        chooseButton = transform.Find("UI/ChooseButton").GetComponent<Button>();
        levelName = transform.Find("UI/LevelName").GetComponent<TextMeshProUGUI>();
        leftButton.onClick.AddListener(async () => await OnLeftClicked());
        rightButton.onClick.AddListener(async () => await OnRightClicked());
        chooseButton.onClick.AddListener(() =>
        {
            BgUI.Instance.HideBg();
            transform.Find("UI").gameObject.SetActive(false);
            GameManager.Instance.GameStream("�ؿ�"+(curLevelIndex+1));
        });
        for (int i = 0; i < levelList.Count; i++)
        {
            positions.Add(levelList[i].transform.position);
            alphas.Add(levelList[i].GetComponent<CanvasGroup>().alpha);
            sizeDelta.Add(levelList[i].GetComponent<RectTransform>().sizeDelta);
        }
        Init(0);
    }
    public void Init(int levelIndex)
    {
        curLevelIndex = levelIndex;
        levelName.text = levelDatabase.levels[curLevelIndex].name;
        if (levelDatabase.levels.Count <= 2)
        {
            levelList[1].GetComponent<Image>().sprite = levelDatabase.levels[1].sprite;
            levelList[2].GetComponent<Image>().sprite = levelDatabase.levels[0].sprite;
            levelList[3].GetComponent<Image>().sprite = levelDatabase.levels[1].sprite;
        }
        if (levelDatabase.levels.Count >= 3)
        {
            levelList[1].GetComponent<Image>().sprite = levelDatabase.levels[levelDatabase.levels.Count - 1].sprite;
            levelList[2].GetComponent<Image>().sprite = levelDatabase.levels[0].sprite;
            levelList[3].GetComponent<Image>().sprite = levelDatabase.levels[1].sprite;
        }
    }

    private async Task OnLeftClicked()
    {
        await MoveTowardsLeft();
        BackToLeftPosition();
        if (curLevelIndex > 0)
            curLevelIndex--;
        else
            curLevelIndex = levelDatabase.levels.Count - 1;
        levelName.text = levelDatabase.levels[curLevelIndex].name;
    }
    private async Task MoveTowardsLeft()
    {
        levelList[0].GetComponent<Image>().sprite = levelDatabase.levels[SetImageForLeftMove()].sprite;
        while (levelList[1].transform.position.x < positions[2].x)
        {
            levelList[0].transform.Translate(Vector3.right * 1.5f);
            levelList[1].transform.Translate(Vector3.right * 3);
            levelList[2].transform.Translate(Vector3.right * 3);
            levelList[3].transform.Translate(Vector3.right * 1.5f);
            levelList[0].GetComponent<CanvasGroup>().alpha += 0.005f;
            levelList[1].GetComponent<CanvasGroup>().alpha += 0.005f;
            levelList[2].GetComponent<CanvasGroup>().alpha -= 0.005f;
            levelList[3].GetComponent<CanvasGroup>().alpha -= 0.005f;
            levelList[0].GetComponent<RectTransform>().sizeDelta = new Vector2(levelList[0].GetComponent<RectTransform>().sizeDelta.x + 1.5f, levelList[0].GetComponent<RectTransform>().sizeDelta.y + 1.5f);
            levelList[1].GetComponent<RectTransform>().sizeDelta = new Vector2(levelList[1].GetComponent<RectTransform>().sizeDelta.x + 1.5f, levelList[1].GetComponent<RectTransform>().sizeDelta.y + 1.5f);
            levelList[2].GetComponent<RectTransform>().sizeDelta = new Vector2(levelList[2].GetComponent<RectTransform>().sizeDelta.x - 1.5f, levelList[2].GetComponent<RectTransform>().sizeDelta.y - 1.5f);
            levelList[3].GetComponent<RectTransform>().sizeDelta = new Vector2(levelList[3].GetComponent<RectTransform>().sizeDelta.x - 1.5f, levelList[3].GetComponent<RectTransform>().sizeDelta.y - 1.5f);
            await Task.Yield();
        }
        BackToLeftPosition();
    }
    private void BackToLeftPosition()
    {
        levelList[0].transform.position = positions[0];
        levelList[1].transform.position = positions[1];
        levelList[2].transform.position = positions[2];
        levelList[3].transform.position = positions[3];
        levelList[0].GetComponent<RectTransform>().sizeDelta = sizeDelta[0];
        levelList[1].GetComponent<RectTransform>().sizeDelta = sizeDelta[1];
        levelList[2].GetComponent<RectTransform>().sizeDelta = sizeDelta[2];
        levelList[3].GetComponent<RectTransform>().sizeDelta = sizeDelta[3];
        levelList[0].GetComponent<CanvasGroup>().alpha = alphas[0];
        levelList[1].GetComponent<CanvasGroup>().alpha = alphas[1];
        levelList[2].GetComponent<CanvasGroup>().alpha = alphas[2];
        levelList[3].GetComponent<CanvasGroup>().alpha = alphas[3];
        levelList[3].GetComponent<Image>().sprite = levelList[2].GetComponent<Image>().sprite;
        levelList[2].GetComponent<Image>().sprite = levelList[1].GetComponent<Image>().sprite;
        levelList[1].GetComponent<Image>().sprite = levelList[0].GetComponent<Image>().sprite;
    }
    private int SetImageForLeftMove()
    {
        if (levelDatabase.levels.Count <= 2)
        {
            if (curLevelIndex == 0)
                return 0;
            else
                return 1;
        }
        else
        {
            if (curLevelIndex > 1)
                return curLevelIndex - 2;
            else if (curLevelIndex == 1)
                return levelDatabase.levels.Count - 1;
            else
                return levelDatabase.levels.Count - 2;
        }
    }

    private async Task OnRightClicked()
    {
        await MoveTowardsRight();
        BackToRightPosition();
        if (curLevelIndex < levelDatabase.levels.Count - 1)
            curLevelIndex++;
        else
            curLevelIndex = 0;
        levelName.text = levelDatabase.levels[curLevelIndex].name;
    }
    private async Task MoveTowardsRight()
    {
        levelList[4].GetComponent<Image>().sprite = levelDatabase.levels[SetImageForRightMove()].sprite;
        while (levelList[3].transform.position.x > positions[2].x)
        {
            levelList[1].transform.Translate(Vector3.left * 1.5f);
            levelList[2].transform.Translate(Vector3.left * 3);
            levelList[3].transform.Translate(Vector3.left * 3);
            levelList[4].transform.Translate(Vector3.left * 1.5f);
            levelList[1].GetComponent<CanvasGroup>().alpha -= 0.005f;
            levelList[2].GetComponent<CanvasGroup>().alpha -= 0.005f;
            levelList[3].GetComponent<CanvasGroup>().alpha += 0.005f;
            levelList[4].GetComponent<CanvasGroup>().alpha += 0.005f;
            levelList[1].GetComponent<RectTransform>().sizeDelta = new Vector2(levelList[1].GetComponent<RectTransform>().sizeDelta.x - 1.5f, levelList[1].GetComponent<RectTransform>().sizeDelta.y - 1.5f);
            levelList[2].GetComponent<RectTransform>().sizeDelta = new Vector2(levelList[2].GetComponent<RectTransform>().sizeDelta.x - 1.5f, levelList[2].GetComponent<RectTransform>().sizeDelta.y - 1.5f);
            levelList[3].GetComponent<RectTransform>().sizeDelta = new Vector2(levelList[3].GetComponent<RectTransform>().sizeDelta.x + 1.5f, levelList[3].GetComponent<RectTransform>().sizeDelta.y + 1.5f);
            levelList[4].GetComponent<RectTransform>().sizeDelta = new Vector2(levelList[4].GetComponent<RectTransform>().sizeDelta.x + 1.5f, levelList[4].GetComponent<RectTransform>().sizeDelta.y + 1.5f);
            await Task.Yield();
        }
        BackToRightPosition();
    }
    private void BackToRightPosition()
    {
        levelList[1].transform.position = positions[1];
        levelList[2].transform.position = positions[2];
        levelList[3].transform.position = positions[3];
        levelList[4].transform.position = positions[4];
        levelList[1].GetComponent<RectTransform>().sizeDelta = sizeDelta[1];
        levelList[2].GetComponent<RectTransform>().sizeDelta = sizeDelta[2];
        levelList[3].GetComponent<RectTransform>().sizeDelta = sizeDelta[3];
        levelList[4].GetComponent<RectTransform>().sizeDelta = sizeDelta[4];
        levelList[1].GetComponent<CanvasGroup>().alpha = alphas[1];
        levelList[2].GetComponent<CanvasGroup>().alpha = alphas[2];
        levelList[3].GetComponent<CanvasGroup>().alpha = alphas[3];
        levelList[4].GetComponent<CanvasGroup>().alpha = alphas[4];
        levelList[1].GetComponent<Image>().sprite = levelList[2].GetComponent<Image>().sprite;
        levelList[2].GetComponent<Image>().sprite = levelList[3].GetComponent<Image>().sprite;
        levelList[3].GetComponent<Image>().sprite = levelList[4].GetComponent<Image>().sprite;
    }
    private int SetImageForRightMove()
    {
        if (levelDatabase.levels.Count <= 2)
        {
            if (curLevelIndex == 0)
                return 0;
            else
                return 1;
        }
        else
        {
            if (curLevelIndex < levelDatabase.levels.Count - 2)
                return curLevelIndex + 2;
            else if (curLevelIndex == levelDatabase.levels.Count - 2)
                return 0;
            else
                return 1;
        }
    }
}