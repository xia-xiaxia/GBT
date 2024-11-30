using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Win : MonoBehaviour
{
    public static Win Instance { get; private set; }

    private Button nextLevelButton;
    private Button levelButton;
    private Button MenuButton;



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
        nextLevelButton = transform.Find("UI/OptionBg/NextLevelButton").GetComponent<Button>();
        levelButton = transform.Find("UI/OptionBg/LevelButton").GetComponent<Button>();
        MenuButton = transform.Find("UI/OptionBg/MenuButton").GetComponent<Button>();
        nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
        levelButton.onClick.AddListener(OnLevelButtonClicked);
        MenuButton.onClick.AddListener(OnMenuButtonClicked);
    }
    public void OnGameWin()
    {
        transform.Find("UI").gameObject.SetActive(true);
    }
    private async void OnNextLevelButtonClicked()
    {
        if (LevelUI.Instance.curLevelIndex == LevelUI.Instance.levelDatabase.levels.Count - 1)
        {
            OnLevelButtonClicked();
        }
        else
        {
            transform.Find("UI").gameObject.SetActive(false);
            await TransitionManager_2.Instance.TransitionIn(1f, 5);
            await SceneManager.UnloadSceneAsync("1.0"/*GameManager.Instance.level*/);
            BgUI.Instance.ShowBg();
            LevelUI.Instance.transform.Find("UI").gameObject.SetActive(true);
            LevelUI.Instance.curLevelIndex++;
            LevelUI.Instance.levelName.text = LevelUI.Instance.levelDatabase.levels[LevelUI.Instance.curLevelIndex].name;
            await Task.Delay(1000);
            await TransitionManager_2.Instance.TransitionOut(5);
        }
    }
    private async void OnLevelButtonClicked()
    {
        transform.Find("UI").gameObject.SetActive(false);
        await TransitionManager_2.Instance.TransitionIn(1f, 5);
        await SceneManager.UnloadSceneAsync("1.0"/*GameManager.Instance.level*/);
        BgUI.Instance.ShowBg();
        LevelUI.Instance.transform.Find("UI").gameObject.SetActive(true);
        await Task.Delay(1000);
        await TransitionManager_2.Instance.TransitionOut(5);
    }
    private async void OnMenuButtonClicked()
    {
        transform.Find("UI").gameObject.SetActive(false);
        await TransitionManager_2.Instance.TransitionIn(1f, 5);
        await SceneManager.UnloadSceneAsync("1.0"/*GameManager.Instance.level*/);
        BgUI.Instance.ShowBg();
        StartUI.Instance.transform.Find("UI").gameObject.SetActive(true);
        await Task.Delay(1000);
        await TransitionManager_2.Instance.TransitionOut(5);
    }
}