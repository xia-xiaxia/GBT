using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AsyncManager : MonoBehaviour
{
    public static AsyncManager Instance { get; private set; }



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    public async Task WaitForMouseClick()
    {
        while (!Input.GetMouseButtonDown(0))
        {
            await Task.Yield(); // 让出主线程，等待下一帧
        }
    }
    public async Task WaitForDreamOver()
    {
        while (EnemyFlowController.Instance.isGameFailed)
        {
            await Task.Yield();
        }
    }
    public async Task WaitForGoalUILoaded()
    {
        while (GoalUI.Instance.transform.Find("UI").gameObject.activeSelf)
        {
            await Task.Yield();
        }
    }
    public async Task WaitForIntroductionUILoaded()
    {
        while (GoalUI.Instance.transform.Find("UI").gameObject.activeSelf)
        {
            await Task.Yield();
        }
    }
    internal async Task WaitForGameEnd()
    {
        switch (GameManager.Instance.level)
        {
            case "1.0":
                while (!(GameFirst.Instance.isWin || GameFirst.Instance.isFaild))
                {
                    await Task.Yield();
                }
                if (GameFirst.Instance.isWin)
                    GameManager.Instance.isWin = true;
                else
                    GameManager.Instance.isWin = false;
                break;
            case "2.0":
                while (!(GameSecond.Instance.isWin || GameSecond.Instance.isFailed))
                {
                    await Task.Yield();
                }
                break;
            case "3.0":
                while (!(GameThird.Instance.isWin || GameThird.Instance.isFaild))
                {
                    await Task.Yield();
                }
                break;
        }
    }
}
