using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager_1 : MonoBehaviour
{
    public static TransitionManager_1 Instance { get; private set; }

    private Image blackScreen;
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
    }
    /// <summary>
    /// ����͸���Ⱥ͵ȴ�������͸����Ϊ1ʱȫ�ڣ��ȴ�����Ĭ��Ϊ�ȴ�1��
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
