using System.Collections;
using System.Collections.Generic;
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
        //Transition();
    }
    private void Transition()
    {
        blackScreen.color = color;
        blackScreen.enabled = true;
        StartCoroutine(TransitionIn());
    }
    public IEnumerator TransitionIn()
    {
        blackScreen.raycastTarget = true;
        while (blackScreen.color.a < 1)
        {
            blackScreen.color += color;
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(TransitionOut());
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
}