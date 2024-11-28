using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroductionBeforeGameManager : MonoBehaviour
{
    public static IntroductionBeforeGameManager Instance { get; private set; }

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
        this.level = level;

        await SceneManager.LoadSceneAsync("1.0", LoadSceneMode.Additive);

        await ShowTutorial();

        //await 回放完毕
        //await SceneManager.UnloadSceneAsync("1.0");

        await ShowGoal();

        //await SceneManager.LoadSceneAsync("XHY", LoadSceneMode.Additive);
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
            await WaitForMouseClick();
            await Task.Delay(100);
        }
        TutorialUI.Instance.transform.Find("UI").gameObject.SetActive(false);
    }
    private async Task ShowGoal()
    {
        TransitionManager.Instance.Transition(0.5f);
        GameObject goal = GameObject.Find("Goal");
        goal.transform.Find("UI").gameObject.SetActive(true);
        LoadTextManager.Instance.LoadText(goal.transform.Find("UI/Text").GetComponent<TextMeshProUGUI>(), level + "目标", goal.transform.Find("UI/Continue").GetComponent<Button>());
    }



    private Task WaitForMouseClick()
    {
        var tcs = new TaskCompletionSource<bool>();
        StartCoroutine(WaitForMouseClickCoroutine(tcs));
        return tcs.Task;
    }

    private IEnumerator WaitForMouseClickCoroutine(TaskCompletionSource<bool> tcs)
    {
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
        tcs.SetResult(true);
    }
}
