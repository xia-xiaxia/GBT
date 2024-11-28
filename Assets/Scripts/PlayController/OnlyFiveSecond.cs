using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyFiveSecond : MonoBehaviour
{
    public float WaitTime;
    public bool isGameOver;

    void Start()
    {
        isGameOver = false;
    }

    void Update()
    {
        if (Input.anyKey) StopCoroutine(OnlyFive());
        else  StartCoroutine(OnlyFive()); 
    }

    IEnumerator OnlyFive()
    {
        yield return new WaitForSeconds(WaitTime);
        isGameOver = true;
    }
}
