using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    private new TextMeshProUGUI name;
    private Image image;

    public void SetInfo(string name, Sprite sprite)
    {
        this.name.text = name;
        this.image.sprite = sprite;
    }
}
