using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCanPlace : MonoBehaviour
{
    public bool canPlace = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canPlace = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canPlace = true;
        }
    }

}
