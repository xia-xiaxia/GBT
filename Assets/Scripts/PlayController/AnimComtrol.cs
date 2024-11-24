using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimComtrol : MonoBehaviour
{
    public GameObject Player;
    public PlayerMovement playerMove;
    private Vector2 playerDir;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        playerDir = playerMove.moveDirection;
        spriteRenderer = GetComponent<SpriteRenderer>();    
    }

    void Update()
    {
        if(playerDir.x > 0) 
            spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;
    }
}
