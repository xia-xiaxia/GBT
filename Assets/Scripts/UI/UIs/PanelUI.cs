using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class PanelUI : MonoBehaviour
{
    public static PanelUI Instance { get; private set; }



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    private async void OnEnable()
    {
        transform.Find("UI").gameObject.SetActive(true);
        await Task.Delay(300);
        InteractableObjectManager.Instance.LoadInteractableObjects();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            transform.Find("UI").gameObject.SetActive(transform.Find("UI").gameObject.activeSelf ? false : true);
        }
    }
}
