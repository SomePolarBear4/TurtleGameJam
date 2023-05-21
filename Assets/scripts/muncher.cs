using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class muncher : MonoBehaviour
{
    public turtleController tutel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Lettuce")
        {
            tutel.MunchTrigger(collision);
        }
    }
}
