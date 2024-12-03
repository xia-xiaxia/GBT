using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelpUI : MonoBehaviour
{
    public static HelpUI Instance { get; private set; }



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
        transform.Find("UI").GetComponent<RectTransform>().sizeDelta = GameObject.Find("Canvas").GetComponent<RectTransform>().sizeDelta;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            transform.Find("UI").gameObject.SetActive(!transform.Find("UI").gameObject.activeSelf);
        }
    }
}
