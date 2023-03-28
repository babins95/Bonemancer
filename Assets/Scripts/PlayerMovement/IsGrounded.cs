using UnityEngine;

public class IsGrounded : MonoBehaviour
{
    public LayerMask groundedLayer;
    public float checkRadius;
    private void Update()
    {
        GetComponentInParent<HeadController>().isGrounded = Physics2D.OverlapCircle(transform.position, checkRadius, groundedLayer);
    }


}
