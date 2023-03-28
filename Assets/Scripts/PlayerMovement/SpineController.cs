using Spine.Unity;
using UnityEngine;
using Spine;

public enum AnimationState { Idle, Walking, VerticalCharging, HorizontalCharging, VerticalJumping, HorizontalJumping, Falling, Landing, Rising}
public class SpineController : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset idle, walking, verticalCharging, horizontalCharging, verticalJumping, horizontalJumping, falling, landing, rising;
    public AnimationState currentState;
    public AnimationState previousState;
    static public string currentAnimation;

    Rigidbody2D spineRigidbody2D;
    Player player;
    public bool isGrounded = false;
    public float speed;
    public float jumpForce;
    private float horizontalMovement;
    private bool jumpButtonPressed;
    public bool isCharging;
    public float chargedJumpPressed;
    public float maxChargedJumpForce;
    public float minChargedJumpForce;
    public float chargedJumpForce;
    private Vector2 chargedJumpDirection;
    public float chargedJumpY;
    public float chargedJumpYHor;
    public float chargedJumpTimer = 0f;
    public float airAcceleration = 1f;
    static public SpineController instance;
    public float fallDeathY = -5f;
    void Start()
    {
        skeletonAnimation.state.Event += state_Event;
        instance = this;
        player = GetComponent<Player>();
        spineRigidbody2D = GetComponent<Rigidbody2D>();
        transform.position = GameManager.instance.lastCheckPointPos;
        currentState = AnimationState.Idle;
        currentAnimation = "idle fermo";
        SetCharacterState(currentState);
    }
    private void FixedUpdate()
    {
        HorizontalMovement();
        Charging();
        if (!jumpButtonPressed && isCharging == true)
        {
            if (horizontalMovement != 0)
            {
                chargedJumpTimer = 2f;
            }
            else
            {
                chargedJumpTimer = 0f;
            }
            if (chargedJumpPressed * chargedJumpForce > maxChargedJumpForce)
            {
                spineRigidbody2D.velocity = chargedJumpDirection * maxChargedJumpForce * Time.deltaTime;
            }
            //else if (horizontalMovement == 0)
            //{
            //    spineRigidbody2D.velocity = chargedJumpDirection * chargedJumpPressed * chargedJumpForce * Time.deltaTime;
            //}
            else
            {
                spineRigidbody2D.velocity = chargedJumpDirection * chargedJumpPressed * chargedJumpForce * Time.deltaTime;
            }
            SetCharacterState(AnimationState.VerticalJumping);

            chargedJumpPressed = 0;
            isCharging = false;
        }
        else if (!jumpButtonPressed && isGrounded == false && chargedJumpPressed > 0)
        {
            chargedJumpTimer = 0;
        }
        else if (!jumpButtonPressed && isCharging == false && isGrounded == true && chargedJumpPressed > 0)
        {
            spineRigidbody2D.velocity = Vector2.up * jumpForce * Time.deltaTime;
            SetCharacterState(AnimationState.VerticalJumping);
            chargedJumpPressed = 0;
        }
        if (horizontalMovement == 0)
        {
            chargedJumpDirection = new Vector2(horizontalMovement, chargedJumpY);
        }
        else
        {
            chargedJumpDirection = new Vector2(horizontalMovement, chargedJumpYHor);
        }
        if (chargedJumpTimer > 0)
        {
            if (spineRigidbody2D.velocity.x == 0)
            {
                chargedJumpTimer = 0;
            }
            else
            {
                chargedJumpTimer -= Time.deltaTime;
            }
        }
        if (isGrounded == true && chargedJumpTimer < 1.5f)
        {
            chargedJumpTimer = 0;
        }
    }

    private void Charging()
    {
        if (isGrounded == true && jumpButtonPressed)
        {
            if (chargedJumpPressed >= minChargedJumpForce)
            {
                isCharging = true;
                if (horizontalMovement != 0) { SetCharacterState(AnimationState.HorizontalCharging); }
                else
                {
                    SetCharacterState(AnimationState.VerticalCharging);
                }
            }
            if (chargedJumpPressed * chargedJumpForce < maxChargedJumpForce)
            {
                chargedJumpPressed += Time.deltaTime;
            }
        }
    }

    private void HorizontalMovement()
    {
        if (isCharging == false && chargedJumpTimer <= 0)
        {
            spineRigidbody2D.velocity = new Vector2(horizontalMovement * speed * Time.deltaTime, spineRigidbody2D.velocity.y);
            if (isGrounded == true && currentState != AnimationState.VerticalJumping)
            {
                if (currentState != AnimationState.Landing)
                {
                    if (horizontalMovement != 0)
                    {
                        SetCharacterState(AnimationState.Walking);
                    }
                    else
                    {
                        SetCharacterState(AnimationState.Idle);
                    }
                }
            }
        }
        else if(chargedJumpTimer > 0)
        {
            if (spineRigidbody2D.velocity.x > 0 && horizontalMovement < 0)
            {
                spineRigidbody2D.AddForce(new Vector2(horizontalMovement, 0) * airAcceleration * Time.deltaTime);
            }
            else if(spineRigidbody2D.velocity.x < 0 && horizontalMovement > 0)
            {
                spineRigidbody2D.AddForce(new Vector2(horizontalMovement, 0) * airAcceleration * Time.deltaTime);
            }
        }

    }

    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        if (horizontalMovement > 0)
        {
            transform.localScale = new Vector2(1f, 1f);
        }
        else if (horizontalMovement < 0)
        {
            transform.localScale = new Vector2(-1f, 1f);
        }
        if (isCharging == true)
        {
            spineRigidbody2D.velocity = new Vector2(0,spineRigidbody2D.velocity.y);
        }
        if (transform.position.y <= fallDeathY)
        {
            player.Death();
        }
        jumpButtonPressed = Input.GetButton("Jump");
        FallingAnimation();
    }
    private void state_Event(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "footstep")
        {
            transform.GetChild(2).GetComponent<AudioSource>().Play();
        }
        if (e.Data.Name == "upcharge" || e.Data.Name == "forwardcharge")
        {
            transform.GetChild(3).GetComponent<AudioSource>().Play();
        }
        if (e.Data.Name == "dash")
        {
            transform.GetChild(6).GetComponent<AudioSource>().Play();
        }

    }
    private void FallingAnimation()
    {
        if (spineRigidbody2D.velocity.y < 0 && isGrounded == false)
        {
            SetCharacterState(AnimationState.Falling);
        }
    }
    //private void Death()
    //{

    //    GameManager.instance.Reload();
    //    Destroy(this.gameObject);

    //}
    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(currentAnimation))
        {
            return;
        }
        Spine.TrackEntry animationEntry = skeletonAnimation.state.SetAnimation(0, animation, loop);
        animationEntry.TimeScale += timeScale;
        animationEntry.Complete += AnimationEntry_Complete;
        currentAnimation = animation.name;
    }

    private void AnimationEntry_Complete(Spine.TrackEntry trackEntry)
    {
        if (currentState == AnimationState.VerticalJumping)
        {
            SetCharacterState(AnimationState.Rising);
        }
        //if(currentState == AnimationState.Falling && isGrounded == true)
        //{
        //    SetCharacterState(AnimationState.Landing);
        //}
        //if (currentState == AnimationState.Falling)
        //{
        //    if (isGrounded == true)
        //    {
        //        SetCharacterState(AnimationState.Landing);
        //    }
        //}
        //if (currentState == AnimationState.Landing)
        //{
        //    SetCharacterState(AnimationState.Idle);
        //}
    }

    public void SetCharacterState(AnimationState state)
    {
        if (state == AnimationState.Idle || state == AnimationState.Walking)
        {
            if(currentState == AnimationState.Falling)
            {
                transform.GetChild(4).GetComponent<AudioSource>().Play();
            }
        }
        currentState = state;


        switch (state)
        {
            case AnimationState.Walking:
                SetAnimation(walking, true, 2f);
                break;
            case AnimationState.VerticalCharging:
                transform.GetChild(1).GetComponent<AudioSource>().Play();
                SetAnimation(verticalCharging, false, 1f);
                break;
            case AnimationState.Rising:
                SetAnimation(rising, false, 1f);
                break;
            case AnimationState.HorizontalCharging:
                SetAnimation(horizontalCharging, false, 1f);
                break;
            case AnimationState.VerticalJumping:
                SetAnimation(verticalJumping, false, 1f);
                break;
            case AnimationState.Falling:
                SetAnimation(falling, false, 1f);
                break;
            case AnimationState.Landing:
                SetAnimation(landing, false, 1f);
                break;
            default:
                SetAnimation(idle, true, 1f);
                break;
        }

        //if(state == AnimationState.Idle)
        //{
        //    SetAnimation(idle, true, 1f);
        //}
        //else if(state == AnimationState.Walking)
        //{
        //    SetAnimation(walking, true, 1f);
        //}
    }
}
