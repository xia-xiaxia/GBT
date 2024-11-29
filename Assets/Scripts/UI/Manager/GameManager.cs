using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isStart = false;
    public static GameManager Instance { get; private set; }

    private Queue<string> tricksQueue;
    private string level;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    public async void IntrodutionBeforeGame(string level)
    {
        try
        {
            isStart = false;
            this.level = level;

            await SceneManager.LoadSceneAsync("1.0", LoadSceneMode.Additive);

            await ShowTutorial();

            await AsyncManager.Instance.WaitForDreamOver();

            await ShowGoal();

            //await SceneManager.LoadSceneAsync("XHY", LoadSceneMode.Additive);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in IntrodutionBeforeGame: {ex.Message}");
        }
    }

    private async Task ShowGoal()
    {
        await TransitionManager_2.Instance.TransitionIn(0.8f);
        await Task.Delay(500);
        GoalUI.Instance.ShowGoal(level);
        await AsyncManager.Instance.WaitForGoalUILoaded();
        await TransitionManager_2.Instance.TransitionOut();
    }


    private async Task ShowTutorial()
    {
        string[] tricks = LoadTextManager.Instance.Texts.Find(t => t.name == level).content;
        tricksQueue = new Queue<string>(tricks);
        TutorialUI.Instance.transform.Find("UI").gameObject.SetActive(true);
        while (tricksQueue.Count > 0)
        {
            string trick = tricksQueue.Dequeue();
            TutorialUI.Instance.ShowTutorial(trick);
            await AsyncManager.Instance.WaitForMouseClick();
            await Task.Delay(100);
        }
        TutorialUI.Instance.transform.Find("UI").gameObject.SetActive(false);

        isStart = true;
    }
}