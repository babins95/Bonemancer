using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaker : MonoBehaviour
{
    public float bounce;
    private float invulnerabilityTime = 2f;
    private float timer;
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() && timer <= 0)
        {
            OctoSkull.instance.hp--;
            timer = invulnerabilityTime;
            Player.instance.AttackSound();
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.up * bounce * Time.deltaTime;
            StartCoroutine("TentacleDamaged");
        }
    }
    IEnumerator TentacleDamaged()
    {
        Player.instance.GetComponent<Rigidbody2D>().velocity = Vector2.up * bounce * Time.deltaTime;
        yield return new WaitForSeconds(0.4f);
        OctoSkull.instance.BgTentacleDeath();
        GetComponent<ChangingAlpha>().alphaChange = false;
        GetComponent<Tentacle>().tookDamage = true;
    }
}
