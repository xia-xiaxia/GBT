using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fail : MonoBehaviour
{
    public static Fail Instance { get; private set; }

    private Button RestartButton;
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
        RestartButton = transform.Find("UI/OptionBg/RestartButton").GetComponent<Button>();
        levelButton = transform.Find("UI/OptionBg/LevelButton").GetComponent<Button>();
        MenuButton = transform.Find("UI/OptionBg/MenuButton").GetComponent<Button>();
        RestartButton.onClick.AddListener(OnRestartButtonClicked);
        levelButton.onClick.AddListener(OnLevelButtonClicked);
        MenuButton.onClick.AddListener(OnMenuButtonClicked);
    }
    public void OnGameFail()
    {
        transform.Find("UI").gameObject.SetActive(true);
    }
    private async void OnRestartButtonClicked()
    {
        transform.Find("UI").gameObject.SetActive(false);
        await AsyncManager.Instance.WaitForUnloadAllScenesButStart();
        BackgroundUI.Instance.ShowBg();
        GameManager.Instance.GameStream((LevelUI.Instance.curLevelIndex + 1)+".0");
    }
    private async void OnLevelButtonClicked()
    {
        await AsyncManager.Instance.WaitForBackToMenu<LevelUI>(this.gameObject);
    }
    private async void OnMenuButtonClicked()
    {
        await AsyncManager.Instance.WaitForBackToMenu<StartUI>(this.gameObject);
    }
}