using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    private Image blackScreen;
    //private GameObject fieldOfView;
    private Color color = new Color(0, 0, 0, 0.01f);



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
        blackScreen = transform.Find("BlackScreen").GetComponent<Image>();
        //fieldOfView = transform.Find("FieldOfView").gameObject;
        //StartCoroutine(Transition(1f, DefaultWaitor()));
    }
    /// <summary>
    /// 传入透明度和等待方法，透明度为1时全黑，等待方法默认为等待1秒
    /// </summary>
    /// <returns></returns>
    public IEnumerator Transition(float transparency, IEnumerator waitor)
    {
        blackScreen.color = color;
        blackScreen.enabled = true;
        yield return StartCoroutine(TransitionIn(transparency));
        yield return waitor;
        yield return StartCoroutine(TransitionOut());
    }
    public IEnumerator Transition(float transparency)
    {
        blackScreen.color = color;
        blackScreen.enabled = true;
        yield return StartCoroutine(TransitionIn(transparency));
        yield return DefaultWaitor();
        yield return StartCoroutine(TransitionOut());
    }
    public IEnumerator TransitionIn(float transparency)
    {
        blackScreen.raycastTarget = true;
        while (blackScreen.color.a < transparency)
        {
            blackScreen.color += color;
            yield return new WaitForSeconds(0.02f);
        }
    }
    public IEnumerator TransitionOut()
    {
        while (blackScreen.color.a > 0)
        {
            blackScreen.color -= color;
            yield return new WaitForSeconds(0.02f);
        }
        blackScreen.raycastTarget = false;
        blackScreen.enabled = false;
    }
    private IEnumerator DefaultWaitor()
    {
        yield return new WaitForSeconds(1f);
    }
}
//public class TransitionManager : MonoBehaviour
//{
//    public static TransitionManager Instance { get; private set; }

//    private Image blackScreen;
//    private Color color = new Color(0, 0, 0, 0.01f);

//    private void Awake()
//    {
//        if (Instance != null && Instance != this)
//        {
//            Destroy(gameObject);
//        }
//        Instance = this;
//    }

//    private void Start()
//    {
//        blackScreen = transform.Find("BlackScreen").GetComponent<Image>();
//    }

//    public async Task Transition(float transparency, IEnumerator waitor)
//    {
//        blackScreen.color = color;
//        blackScreen.enabled = true;
//        await StartCoroutineAsync(TransitionIn(transparency));
//        yield return waitor;
//        await StartCoroutineAsync(TransitionOut());
//    }

//    public IEnumerator TransitionIn(float transparency)
//    {
//        blackScreen.raycastTarget = true;
//        while (blackScreen.color.a < transparency)
//        {
//            blackScreen.color += color;
//            yield return new WaitForSeconds(0.02f);
//        }
//    }

//    public IEnumerator TransitionOut()
//    {
//        while (blackScreen.color.a > 0)
//        {
//            blackScreen.color -= color;
//            yield return new WaitForSeconds(0.02f);
//        }
//        blackScreen.raycastTarget = false;
//        blackScreen.enabled = false;
//    }

//    private IEnumerator DefaultWaitor()
//    {
//        yield return new WaitForSeconds(1f);
//    }

//    private Task StartCoroutineAsync(IEnumerator routine)
//    {
//        var tcs = new TaskCompletionSource<bool>();
//        StartCoroutine(RunCoroutine(routine, tcs));
//        return tcs.Task;
//    }

//    private IEnumerator RunCoroutine(IEnumerator routine, TaskCompletionSource<bool> tcs)
//    {
//        yield return routine;
//        tcs.SetResult(true);
//    }
//}