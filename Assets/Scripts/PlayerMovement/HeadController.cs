using System;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    public float speed = 1f;
    public float soundSpeed;
    private float horizontal;
    private bool jumpButtonPressed;
    public float jumpForce = 2;
    public float maxMoveSpeed = 10f;
    public float rotateSpeed = 10f;
    public bool isGrounded = false;
    Rigidbody2D rigidbody2D;
    public AudioSource jump;
    public AudioSource respawn;
    AudioSource moving;
    Player player;
    public float fallDeathY = -5f;
    void Start()
    {
        respawn.Play();
        rigidbody2D = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        transform.position = GameManager.instance.lastCheckPointPos;
        moving = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        MoveSound();
        //rigidbody2D.AddForce(new Vector3(horizontal, 0) * speed * Time.deltaTime);
        //rigidbody2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, rigidbody2D.velocity.y);
        horizontal = Input.GetAxisRaw("Horizontal");
        jumpButtonPressed = Input.GetButtonDown("Jump");
        if (jumpButtonPressed && isGrounded == true)
        {
            Jump();
            jump.Play();
        }
        MaxSpeedLimit();
        if (transform.position.y <= fallDeathY)
        {
            player.Death();
        }
    }
    private void MoveSound()
    {
        if (isGrounded == false)
        {
            moving.Stop();
        }
        else if (moving.isPlaying == false && isGrounded == true)
        {
            moving.Play();
        }
        moving.pitch = rigidbody2D.velocity.magnitude / soundSpeed;
    }

    //public void Death()
    //{

    //    GameManager.instance.Reload();
    //    Destroy(this.gameObject);

    //}

    private void Jump()
    {
        rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        HorizontalMovement();
        //rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity, maxMoveSpeed);
        //if (rigidbody2D.velocity.magnitude > maxMoveSpeed)
        //{
        //    rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity, maxMoveSpeed);
        //}
    }

    private void MaxSpeedLimit()
    {
        if (rigidbody2D.velocity.magnitude > maxMoveSpeed)
        {
            rigidbody2D.velocity = rigidbody2D.velocity.normalized * maxMoveSpeed;
        }
    }

    private void HorizontalMovement()
    {
        rigidbody2D.AddForce(new Vector2(horizontal, 0) * speed * Time.deltaTime);
        rigidbody2D.rotation -= horizontal * rotateSpeed;

    }
}
