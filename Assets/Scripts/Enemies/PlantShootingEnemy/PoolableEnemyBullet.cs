using UnityEngine;

public class PoolableEnemyBullet : PoolableObject
{
    public float enemyBulletSpeed = 5f;
    private DisableAfterTime _disableAfterTime;
    private Rigidbody2D _rb;
    public Vector3 bulletAngle;

    void OnEnable()
    {
        _disableAfterTime = GetComponent<DisableAfterTime>();
        _rb = GetComponent<Rigidbody2D>();
    }
    public override void OnCloneActivated()
    {
        _disableAfterTime.ResetTime();
        _rb.velocity = Vector3.zero;
    }
    private void Update()
    {
        transform.position += enemyBulletSpeed * Time.deltaTime * bulletAngle;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.Death();
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
