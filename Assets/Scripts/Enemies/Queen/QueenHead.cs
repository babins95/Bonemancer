using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenHead : MonoBehaviour
{
    public float bounce;
    private void Update()
    {
        //if (SpineController.instance.isGrounded == true)
        //{
        //    SpineController.instance.enabled = true;
        //}
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            SpineController.instance.chargedJumpTimer = 2f;
            Player.instance.GetComponent<Rigidbody2D>().velocity = new Vector2(Player.instance.transform.localScale.x, 1) * bounce * Time.deltaTime;
            QueenStateMachine.instance.TakeDamage();
        }
    }
}
