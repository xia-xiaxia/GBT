using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimComtrol : MonoBehaviour
{
    public GameObject Player;
    public PlayerMovement playerMove;
    private Vector2 playerDir;
    private 

    void Start()
    {
        playerDir = playerMove.moveDirection;
    }

    void Update()
    {
        if(playerDir.x > 0) 
            Player.

    }
}
