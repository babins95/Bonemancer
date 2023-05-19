using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactite : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    public AudioSource stalactiteCrush;
    private bool audioIsPlaying = false;
    public bool isfalling = false;
    public float stalactiteTimer;
    private float timer;
    private Vector3 startingPosition;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(isfalling == true)
        {
            audioIsPlaying = true;
            timer = stalactiteTimer;
            rigidbody2D.isKinematic = false;
            isfalling = false;

        }
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            rigidbody2D.isKinematic = true;
            transform.position = startingPosition;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(audioIsPlaying == true)
        {
            stalactiteCrush.Play();
            audioIsPlaying = false;
        }
    }
}
