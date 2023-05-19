using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenBullet : MonoBehaviour
{
    public float queenBulletSpeed = 5f;
    public Vector3 bulletAngle;
    static public int bulletToSpawn = 1;
    private void Update()
    {
        transform.position += queenBulletSpeed * Time.deltaTime * bulletAngle;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        QueenHead queenHead = collision.GetComponent<QueenHead>();
        QueenBullet queenBullet = collision.GetComponent<QueenBullet>();
        if (player != null)
        {
            player.Death();
            Destroy(gameObject);
        }
        else if(queenHead != null || queenBullet != null)
        {

        }
        else
        {
            Destroy(gameObject);
        }
    }
}
