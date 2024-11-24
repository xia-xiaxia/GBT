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
    private Button endButton;



    private void Start()
    {
        startButton = transform.Find("StartButton").GetComponent<Button>();
        endButton = transform.Find("EndButton").GetComponent<Button>();
        startButton.onClick.AddListener(OnStartButtonClicked);
        endButton.onClick.AddListener(OnEndButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        SceneManager.LoadScene("XHY");
    }
    private void OnEndButtonClicked()
    {
        Application.Quit();
    }
}
