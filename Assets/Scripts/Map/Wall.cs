using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Player")
        {

        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.name == "Player")
        {
 
        }
    }

    // Åö×²ÍË³ö
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.name == "Player")
        {

        }
    }
}
