using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    private float timeElapsed;
    public float breakTimer;
    private bool breakStart = false;
    public float respawn;
    private bool soundStart = false;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timeElapsed = breakTimer;
    }
    void Update()
    {
        if(breakStart==true)
        {
            timeElapsed -= Time.deltaTime;
        }
        if (timeElapsed <= 0)
        {
            soundStart = true;
            StartCoroutine("PlatformBreak");
        }
        if(soundStart == true)
        {
            audioSource.Play();
        }
    }

    IEnumerator PlatformBreak()
    {
        soundStart = false;
        transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().enabled = false;
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(respawn);
        transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().enabled = true;
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
        timeElapsed = breakTimer;
        breakStart = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IsGrounded player = collision.GetComponent<IsGrounded>();
        if (player != null)
        {
            breakStart = true;
        }
        SpineIsGrounded playerSpine = collision.GetComponent<SpineIsGrounded>();
        if (playerSpine != null)
        {
            breakStart = true;
        }
    }
}
