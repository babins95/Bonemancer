using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenMortarShoot : MonoBehaviour
{

    public float mortarBulletSpeed = 5f;
    private Rigidbody2D rigidbody2D;
    public Vector2 direction;
    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        if (gameObject.transform.position.x - Player.instance.transform.position.x < 0)
        {
            direction = new Vector2(-direction.x, direction.y);
        }
        rigidbody2D.velocity = direction * mortarBulletSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.Death();
            Destroy(gameObject);
        }
        else if(collision.GetComponent<CompositeCollider2D>())
        {
            transform.GetChild(0).gameObject.SetActive(true);
            Destroy(rigidbody2D);
            StartCoroutine("TimeElapsed");
        }
    }
    IEnumerator TimeElapsed()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
