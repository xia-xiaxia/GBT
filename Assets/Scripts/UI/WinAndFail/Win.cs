using System;
using System.Collections;
using System.Collections.Generic;
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
        StartCoroutine(TransitionManager.Instance.Transition(1f));//Âß¼­Òª¸Ä
        await SceneManager.LoadSceneAsync("Level");
    }
    private void OnLevelButtonClicked()
    {
        SceneManager.LoadScene("Level");
    }
    private void OnMenuButtonClicked()
    {
        SceneManager.LoadScene("Start");
    }
}
//private async void OnNextLevelButtonClicked()
//{
//    var waitor = TransitionManager.Instance.DefaultWaitor();
//    var transitionTask = TransitionManager.Instance.Transition(1f, waitor);
//    var loadSceneTask = SceneManager.LoadSceneAsync("Level");

//    await transitionTask;
//    await loadSceneTask;
//}