using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    private Button startButton;
    private Button bgButton;
    private Button exitButton;



    private void Start()
    {
        startButton = transform.Find("StartButton").GetComponent<Button>();
        bgButton = transform.Find("BgButton").GetComponent<Button>();
        exitButton = transform.Find("ExitButton").GetComponent<Button>();
        startButton.onClick.AddListener(OnStartButtonClicked);
        bgButton.onClick.AddListener(OnStartButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        SceneManager.LoadScene("XHY");
    }
    private void OnBgButtonClicked()
    {
        
    }
    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
