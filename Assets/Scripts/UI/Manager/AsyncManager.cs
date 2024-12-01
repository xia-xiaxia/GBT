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
        while (EnemyFlowController.Instance.isExecuting)
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
        throw new NotImplementedException();
    }
}
