using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public async Task WaitForDemoOver(CancellationToken cancellationToken)
    {
        switch (GameManager.Instance.level)
        {
            case "1.0":
                while (!EnemyFlowController.Instance.isGameFailed)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    //cancellationToken.ThrowIfCancellationRequested();
                    await Task.Yield();
                }
                break;
            case "2.0":
                while (!EnemyController.Instance.isGameFailed)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    //cancellationToken.ThrowIfCancellationRequested();
                    await Task.Yield();
                }
                break;
            case "3.0":
                while (!EnemyFlowController.Instance.isGameFailed)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    //cancellationToken.ThrowIfCancellationRequested();
                    await Task.Yield();
                }
                break;
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
        while (IntroductionUI.Instance.transform.Find("UI").gameObject.activeSelf)
        {
            await Task.Yield();
        }
    }
    public async Task WaitForGameEnd(CancellationToken cancellationToken)
    {
        switch (GameManager.Instance.level)
        {
            case "1.0":
                while (!(GameFirst.Instance.isWin || GameFirst.Instance.isFaild))
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    //cancellationToken.ThrowIfCancellationRequested();
                    await Task.Yield();
                }
                if (GameFirst.Instance.isWin)
                    GameManager.Instance.isWin = true;
                else
                    GameManager.Instance.isWin = false;
                break;
            case "2.0":
                while (!(GameThird.Instance.isWin || GameThird.Instance.isFailed))
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    //cancellationToken.ThrowIfCancellationRequested();
                    await Task.Yield();
                }
                if (GameThird.Instance.isWin)
                    GameManager.Instance.isWin = true;
                else
                    GameManager.Instance.isWin = false;
                break;
            case "3.0":
                while (!(GameSecond.Instance.isWin || GameSecond.Instance.isFailed))
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    //cancellationToken.ThrowIfCancellationRequested();
                    await Task.Yield();
                }
                if (GameSecond.Instance.isWin)
                    GameManager.Instance.isWin = true;
                else
                    GameManager.Instance.isWin = false;
                break;
        }
    }
    public async Task WaitForUnloadAllScenesButStart()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != "Start")
            {
                await SceneManager.UnloadSceneAsync(scene);
            }
        }
    }
    public async Task WaitForBackToMenu<T>(GameObject go) where T : MonoBehaviour
    {
        go.transform.Find("UI").gameObject.SetActive(false);
        GameManager.Instance.EnableOrDisableAllUIsInGame(false, null);
        await TransitionManager_2.Instance.TransitionIn(1f, 5);
        await WaitForUnloadAllScenesButStart();
        BackgroundUI.Instance.ShowBg();

        // 使用反射获取静态Instance属性
        var instanceProperty = typeof(T).GetProperty("Instance", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        if (instanceProperty == null)
        {
            throw new InvalidOperationException($"Type {typeof(T).Name} does not have a public static Instance property.");
        }
        var instance = instanceProperty.GetValue(null) as T;
        if (instance == null)
        {
            throw new InvalidOperationException($"Instance of type {typeof(T).Name} is null.");
        }
        instance.transform.Find("UI").gameObject.SetActive(true);

        await Task.Delay(1000);
        await TransitionManager_2.Instance.TransitionOut(5);
    }
}
