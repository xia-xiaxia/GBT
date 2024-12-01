using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
    [HideInInspector]
    public TextMeshProUGUI levelName;
    private GameObject complete;

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
        complete = transform.Find("UI/Complete").gameObject;
        leftButton.onClick.AddListener(async () => await OnLeftClicked());
        rightButton.onClick.AddListener(async () => await OnRightClicked());
        chooseButton.onClick.AddListener(() =>
        {
            transform.Find("UI").gameObject.SetActive(false);
            GameManager.Instance.GameStream((curLevelIndex + 1) + ".0");
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
        complete.SetActive(GameManager.Instance.completionRecord[curLevelIndex]);
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
        complete.SetActive(GameManager.Instance.completionRecord[curLevelIndex]);
    }
    private async Task MoveTowardsLeft()
    {
        levelList[0].GetComponent<Image>().sprite = levelDatabase.levels[SetImage(curLevelIndex - 2)].sprite;
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
        levelList[3].GetComponent<Image>().sprite = levelDatabase.levels[SetImage(curLevelIndex)].sprite;
        levelList[2].GetComponent<Image>().sprite = levelDatabase.levels[SetImage(curLevelIndex - 1)].sprite;
        levelList[1].GetComponent<Image>().sprite = levelDatabase.levels[SetImage(curLevelIndex - 2)].sprite;
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
        complete.SetActive(GameManager.Instance.completionRecord[curLevelIndex]);
    }
    private async Task MoveTowardsRight()
    {
        levelList[4].GetComponent<Image>().sprite = levelDatabase.levels[SetImage(curLevelIndex + 2)].sprite;
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
        levelList[1].GetComponent<Image>().sprite = levelDatabase.levels[SetImage(curLevelIndex)].sprite;
        levelList[2].GetComponent<Image>().sprite = levelDatabase.levels[SetImage(curLevelIndex + 1)].sprite;
        levelList[3].GetComponent<Image>().sprite = levelDatabase.levels[SetImage(curLevelIndex + 2)].sprite;
    }

    private int SetImage(int index)
    {
        if (index < 0)
            return levelDatabase.levels.Count + index;
        else if (index >= levelDatabase.levels.Count)
            return index - levelDatabase.levels.Count;
        else
            return index;
    }
}