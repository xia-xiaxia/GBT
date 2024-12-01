using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Vector3[] wallPositions;
    GameObject[] Walls;
    public Box box;
    public PlayerMovement PM;
    public bool canTrans;

    void Start()
    {
        Walls = GameObject.FindGameObjectsWithTag("Wall");

        foreach (GameObject wall in Walls)
        {
            for (int i = 0; i < Walls.Length; i++)
            {
                wallPositions[i] = wall.transform.position;
            }
        }

    }

    void Update()
    {
        boxpositionExamine(box.targetPosition);
    }

    void boxpositionExamine(Vector3 boxPosition)
    {
        foreach (GameObject wall in Walls)
        {
            if (boxPosition == wall.transform.position)
            {
                canTrans = false;
            }
        }
    }

}