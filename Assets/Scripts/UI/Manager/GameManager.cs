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
    public static GameManager Instance { get; private set; }

    private Queue<string> tricksQueue;
    public string level;
    public List<bool> completionRecord;



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
        int levelCount = LevelUI.Instance.levelDatabase.levels.Count;
        completionRecord = new List<bool>(levelCount);
        for (int i = 0; i < levelCount; i++)
        {
            completionRecord.Add(false);
        }
    }
    public async void GameStream(string level)
    {
        this.level = level;

        await ShowIntroduction();

        await SceneManager.LoadSceneAsync(level + "demo", LoadSceneMode.Additive);
        await TransitionManager_2.Instance.TransitionOut(5);
        await AsyncManager.Instance.WaitForDreamOver();

        await ShowGoal();

        await ShowTutorial();

        await SceneManager.UnloadSceneAsync(level + "demo");
        await SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);

        GameObject.Find("Canvas").transform.Find("PanelUI").gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.Find("HelpUI").gameObject.SetActive(true);

        await AsyncManager.Instance.WaitForGameEnd();

        GameObject.Find("Canvas").transform.Find("PanelUI").gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.Find("HelpUI").gameObject.SetActive(false);

        await ShowGameOver(true);
    }

    private async Task ShowIntroduction()
    {
        await TransitionManager_2.Instance.TransitionIn(1f, 10);
        BgUI.Instance.HideBg();
        await Task.Delay(500);
        IntrouctionUI.Instance.ShowIntroduction(level);
        await AsyncManager.Instance.WaitForIntroductionUILoaded();
    }
    private async Task ShowGoal()
    {
        await TransitionManager_2.Instance.TransitionIn(0.8f, 10);
        await Task.Delay(500);
        GoalUI.Instance.ShowGoal(level);
        await AsyncManager.Instance.WaitForGoalUILoaded();
    }
    private async Task ShowTutorial()
    {
        TransitionManager_2.Instance.up.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        TransitionManager_2.Instance.down.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        var tutorialText = LoadTextManager.Instance.Texts.Find(t => t.name == level + "ÐÂÔöÍæ·¨");
        if (tutorialText != null)
        {
            string[] tricks = tutorialText.content;
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
        }
        await TransitionManager_2.Instance.TransitionOut(5);
    }
    private async Task ShowGameOver(bool isWin)
    {
        if (isWin)
        {
            bool isAllCompleted = false;
            foreach (bool b in completionRecord)
            {
                if (!b)
                {
                    isAllCompleted = false;
                    break;
                }
            }
            if (!isAllCompleted)
            {
                completionRecord[LevelUI.Instance.curLevelIndex] = true;
                Win.Instance.OnGameWin();
            }
            else
                CompletionUI.Instance.transform.Find("UI").gameObject.SetActive(true);
        }
        else
        {
            Fail.Instance.OnGameFail();
        }
    }
}