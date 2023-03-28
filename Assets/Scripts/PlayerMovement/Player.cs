using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    static public Player instance;
    public AudioSource death;
    public AudioSource octoSkullLaugh;
    public AudioSource playerAttack;
    public bool isInBossFight = false;
    private void Start()
    {
        instance = this;
    }
    public void Death()
    {
        death.Play();
        if(isInBossFight == true)
        {
            if (octoSkullLaugh != null)
            {
                octoSkullLaugh.Play();
            }
        }
        GameManager.instance.Reload();
        Destroy(this.gameObject);

    }
    public void AttackSound()
    {
        playerAttack.Play();
    }
}
