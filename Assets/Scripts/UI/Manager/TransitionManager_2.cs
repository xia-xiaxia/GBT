using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager_2 : MonoBehaviour
{
    public static TransitionManager_2 Instance { get; private set; }

    private GameObject up;
    private GameObject down;

    private float transparency;
    private float maxY;
    private float minY;
    private float x;
    private float a;



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
        GetComponent<RectTransform>().sizeDelta = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta;
        up = transform.Find("Up").gameObject;
        down = transform.Find("Down").gameObject;
        up.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, up.GetComponent<RectTransform>().sizeDelta.y / 2 + 20, 0);
        down.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -down.GetComponent<RectTransform>().sizeDelta.y / 2 - 20, 0);
        maxY = (up.GetComponent<RectTransform>().position.y - transform.position.y) * 2 + 7.1375f;
        minY = up.GetComponent<RectTransform>().sizeDelta.y;
        x = up.GetComponent<RectTransform>().sizeDelta.x;
        a = up.GetComponent<Image>().color.a;
    }
    /// <summary>
    /// 传入透明度和等待方法，透明度为1时全黑，等待方法默认为等待1秒
    /// </summary>
    /// <returns></returns>
    public async Task TransitionIn(float transparency)
    {
        while (up.GetComponent<RectTransform>().sizeDelta.y < maxY - 7.2f)
        {
            float y = up.GetComponent<RectTransform>().sizeDelta.y;
            float t = up.GetComponent<Image>().color.a;
            float newY = Mathf.Lerp(y, maxY, 0.01f);
            float newA = Mathf.Lerp(t, transparency, 0.01f);
            up.GetComponent<RectTransform>().sizeDelta = new Vector2(x, newY);
            down.GetComponent<RectTransform>().sizeDelta = new Vector2(x, newY);
            up.GetComponent<Image>().color = new Color(0, 0, 0, newA);
            down.GetComponent<Image>().color = new Color(0, 0, 0, newA);
            await Task.Yield();
        }
        up.GetComponent<Image>().color = new Color(0, 0, 0, transparency);
        down.GetComponent<Image>().color = new Color(0, 0, 0, transparency);
    }
    public async Task TransitionOut()
    {
        while (up.GetComponent<RectTransform>().sizeDelta.y > minY + 7.2f)
        {
            float y = up.GetComponent<RectTransform>().sizeDelta.y;
            float t = up.GetComponent<Image>().color.a;
            float newY = Mathf.Lerp(y, minY, 0.01f);
            float newA = Mathf.Lerp(t, a, 0.01f);
            up.GetComponent<RectTransform>().sizeDelta = new Vector2(x, newY);
            down.GetComponent<RectTransform>().sizeDelta = new Vector2(x, newY);
            up.GetComponent<Image>().color = new Color(0, 0, 0, newA);
            down.GetComponent<Image>().color = new Color(0, 0, 0, newA);
            await Task.Yield();
        }
        up.GetComponent<RectTransform>().sizeDelta = new Vector2(x, minY);
        down.GetComponent<RectTransform>().sizeDelta = new Vector2(x, minY);
        up.GetComponent<Image>().color = new Color(0, 0, 0, a);
        down.GetComponent<Image>().color = new Color(0, 0, 0, a);
    }
    private IEnumerator DefaultWaitor()
    {
        yield return new WaitForSeconds(1f);
    }
}
