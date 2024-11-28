using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsyncManager : MonoBehaviour
{
    public static AsyncManager Instance { get; private set; }



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    public async void WaitForMouseClick()
    {

    }
}
