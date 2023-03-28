using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IsGrounded player = collision.GetComponent<IsGrounded>();
        if(player!= null)
        {
            GameManager.instance.SetCheckpoint(this);
        }
        SpineIsGrounded playerSpine = collision.GetComponent<SpineIsGrounded>();
        if (playerSpine != null)
        {
            GameManager.instance.SetCheckpoint(this);
        }
    }
}
