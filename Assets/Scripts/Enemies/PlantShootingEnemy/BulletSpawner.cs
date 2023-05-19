using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public EnemyBulletObjectPool bulletObjectPool;
    public void Shoot()
    {
        GameObject newBullet = bulletObjectPool.Get();
        if (newBullet != null)
        {
            newBullet.transform.position = transform.position;
            newBullet.transform.rotation = transform.rotation;
        }
    }
}
