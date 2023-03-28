using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    public AudioSource octoSkullAttack;
    public AudioSource octoSkullRetreat;

    private Animator animator;

    public Color waitingColor;
    public bool isAttacking = false;
    private bool isReturning = false;
    public bool tookDamage = false;
    public bool waiting = false;
    public float waitTimer;
    private float timer = 0;
    private Vector3 startingPosition;
    public Transform groundPosition;
    public float tentacleSpeed;
    public float returnSpeed;
    public List<Stalactite> stalactites = new List<Stalactite>();
    private ChangingAlpha changingAlpha;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        changingAlpha = GetComponent<ChangingAlpha>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waiting == true)
        {
            changingAlpha.spriteColor = waitingColor;
            isAttacking = false;
        }
        else
        {
            changingAlpha.spriteColor = changingAlpha.startingColor;
        }
        if(timer != 0)
        {
            timer -= Time.deltaTime;
        }
        if (isAttacking == true && waiting == false)
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("isIdle", false);
            if (transform.position == startingPosition)
            {
                octoSkullAttack.Play();
            }
            if (transform.position != groundPosition.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, groundPosition.position, tentacleSpeed * Time.deltaTime);
            }
            else if(transform.position == groundPosition.position)
            {
                BossFightStart.instance.isShaking = true;
                animator.SetBool("isAttacking", false);
                isAttacking = false;
                foreach(Stalactite s in stalactites)
                {
                    s.isfalling = true;
                }
                timer = waitTimer;
            }
        }
        if(timer < 0)
        {
            isReturning = true;
            timer = 0;
        }
        if(isReturning == true && tookDamage == false)
        {
            if(transform.position == groundPosition.position)
            {
                octoSkullRetreat.Play();
                animator.SetBool("isRetreating", true);
            }
            if (transform.position != startingPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, startingPosition, returnSpeed * Time.deltaTime);
            }
            else if (transform.position == startingPosition)
            {
                animator.SetBool("isRetreating", false);
                animator.SetBool("isIdle", true);
                isReturning = false;
            }
        }
        else if (tookDamage == true)
        {
            if (transform.position != startingPosition)
            {
                animator.SetBool("isRetreating", true);
                transform.position = Vector3.MoveTowards(transform.position, startingPosition, tentacleSpeed * Time.deltaTime);
            }
            else if (transform.position == startingPosition)
            {
                animator.SetBool("isRetreating", false);
                animator.SetBool("isIdle", true);
                tookDamage = false;
            }
        }
    }
}
