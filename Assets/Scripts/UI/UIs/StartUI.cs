using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    public static StartUI Instance { get; private set; }

    private Button startButton;
    private Button bgButton;
    private Button exitButton;



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
        startButton = transform.Find("UI/StartButton").GetComponent<Button>();
        bgButton = transform.Find("UI/BgButton").GetComponent<Button>();
        exitButton = transform.Find("UI/ExitButton").GetComponent<Button>();
        startButton.onClick.AddListener(OnStartButtonClicked);
        bgButton.onClick.AddListener(OnBgButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        transform.Find("UI").gameObject.SetActive(false);
        LevelUI.Instance.transform.Find("UI").gameObject.SetActive(true);
    }
    private void OnBgButtonClicked()
    {
        print("NoBackground!");
    }
    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
