using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSlash : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            collision.GetComponent<Player>().Death();
        }
    }
}
