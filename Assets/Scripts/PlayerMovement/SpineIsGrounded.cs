using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineIsGrounded : MonoBehaviour
{
    public LayerMask groundedLayer;
    public float checkRadius;
    private void Update()
    {
        GetComponentInParent<SpineController>().isGrounded = Physics2D.OverlapCircle(transform.position, checkRadius, groundedLayer);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<MovingPlatform>())
        {
            SpineController.instance.transform.parent = collision.gameObject.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<MovingPlatform>())
        {
            SpineController.instance.transform.parent = null;
        }
    }
}
