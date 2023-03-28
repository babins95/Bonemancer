using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    public float bounce;
    public AudioSource death;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Player>())
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.up * bounce * Time.deltaTime;
            StartCoroutine("HeadDamaged");
        }
    }
    IEnumerator HeadDamaged()
    {
        Player.instance.GetComponent<Rigidbody2D>().velocity = Vector2.up * bounce * Time.deltaTime;
        Player.instance.AttackSound();
        yield return new WaitForSeconds(0.1f);
        death.Play();
        Destroy(this.transform.parent.gameObject);
    }
}
