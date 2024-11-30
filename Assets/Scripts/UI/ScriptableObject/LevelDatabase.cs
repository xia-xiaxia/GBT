using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelDatabase : ScriptableObject
{
    public List<LevelData> levels;
}

[Serializable]
public class LevelData
{
    public string name;
    public Sprite sprite;
}