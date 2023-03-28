using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private Vector3 startPos;
    public GameObject camera;
    public float parallaxEffect;
    public bool bossRoom = false;
    static public ParallaxEffect instance;

    void Start()
    {
        instance = this;
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        float temp = (camera.transform.position.x * (1 - parallaxEffect));
        float dist = (camera.transform.position.x * parallaxEffect);
        if (bossRoom == false)
        {
            transform.position = new Vector3(startPos.x + dist, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, parallaxEffect);
        }
    }
}
