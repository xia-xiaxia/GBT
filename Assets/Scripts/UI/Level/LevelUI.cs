using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUI : MonoBehaviour
{
    public static LevelUI Instance { get; private set; }

    public LevelDatabase levelDatabase;
    public GameObject levelPrefab;



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
        for (int i = 0; i < levelDatabase.levels.Count; i++)
        {
            GameObject level = Instantiate(levelPrefab, transform.Find("UI"));
            level.GetComponent<Level>().SetInfo(levelDatabase.levels[i].name, levelDatabase.levels[i].image);
        }
    }
}