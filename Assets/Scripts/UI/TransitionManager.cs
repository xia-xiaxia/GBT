using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    public Image image;
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
        image.color = color;
        image.enabled = true;
        StartCoroutine(TransitionIn());
    }
    public IEnumerator TransitionIn()
    {
        image.raycastTarget = true;
        while (image.color.a < 1)
        {
            image.color += color;
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(TransitionOut());
    }
    public IEnumerator TransitionOut()
    {
        while (image.color.a > 0)
        {
            image.color -= color;
            yield return new WaitForSeconds(0.02f);
        }
        image.raycastTarget = false;
        image.enabled = false;
    }
}