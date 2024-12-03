using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ESCUI : MonoBehaviour
{
    public static ESCUI Instance { get; private set; }

    private Button ESCButton;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    private void OnEnable()
    {
        transform.Find("UI").gameObject.SetActive(false);
    }
    private void Start()
    {
        ESCButton = transform.Find("UI/MenuButton").GetComponent<Button>();
        ESCButton.onClick.AddListener(OnESCButtonClicked);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            transform.Find("UI").gameObject.SetActive(!transform.Find("UI").gameObject.activeSelf);
        }
    }
    private async void OnESCButtonClicked()
    {
        GameManager.Instance.cancellationTokenSource.Cancel();

        transform.Find("UI").gameObject.SetActive(false);
        GameManager.Instance.EnableOrDisableAllUIsInGame(false, new List<string> { "ESCUI" });
        await TransitionManager_2.Instance.TransitionIn(1f, 5);
        await AsyncManager.Instance.WaitForUnloadAllScenesButStart();
        BackgroundUI.Instance.ShowBg();
        StartUI.Instance.transform.Find("UI").gameObject.SetActive(true);
        await Task.Delay(1000);
        await TransitionManager_2.Instance.TransitionOut(5);
        this.gameObject.SetActive(false);
    }
}
