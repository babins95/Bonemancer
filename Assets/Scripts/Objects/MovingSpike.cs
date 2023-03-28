using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpike : MonoBehaviour
{
    public Transform pos1, pos2;
    public float speed;
    public Transform startPosition;

    Vector3 nextPosition;
    void Start()
    {
        nextPosition = startPosition.position;
    }
    void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        if (transform.position == pos1.position)
        {
            nextPosition = pos2.position;
        }
        if (transform.position == pos2.position)
        {
            nextPosition = pos1.position;
        }
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            collision.GetComponent<Player>().Death();
        }
    }
}
