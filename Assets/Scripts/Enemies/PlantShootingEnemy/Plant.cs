using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class Plant : MonoBehaviour
{
    private SkeletonAnimation skeletonAnimation;
    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.state.Event += state_Event;
    }
    private void state_Event(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "shoot")
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<BulletSpawner>().Shoot();
            }
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
