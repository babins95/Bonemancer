using UnityEngine;
using Spine.Unity;
using Spine;

public enum EnemyAnimationState { Idle, Walking, Dashing, Melee }

public class EnemyAgent : MonoBehaviour
{
    public AnimationReferenceAsset idle, walking, dashing, melee;
    public SkeletonAnimation skeletonAnimation;
    public EnemyAnimationState currentState;
    public string currentAnimation;

    public Transform castPos;

    const string LEFT = "left";
    const string RIGHT = "right";
    string facingDirection;

    Vector3 baseScale;

    public float baseCastDist;

    Rigidbody2D rBody;

    public GameObject target;

    public float dashSpeed = 50f;
    public float startingDashTimer = 0.1f;
    public float dashTimer = 0f;
    private float resetTimer = 0f;
    public float startingResetTimer = 2f;
    private bool isDashing = false;
    public float moveSpeed = 5f;

    public float startingAttackTimer = 0.2f;
    private float attackTimer;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;
    public Transform attackPoint;
    void Start()
    {
        skeletonAnimation.state.Event += state_Event;
        baseScale = transform.localScale;

        facingDirection = RIGHT;

        rBody = GetComponent<Rigidbody2D>();

        currentState = EnemyAnimationState.Idle;
        currentAnimation = "idle";
        SetCharacterState(currentState);
    }
    void Update()
    {
        if (currentState != EnemyAnimationState.Idle)
        {
            MeleeAttack();
        }
        if (resetTimer <= 0)
        {
            dashTimer = startingDashTimer;
            resetTimer = startingResetTimer;
        }
        if (dashTimer <= 0)
        {
            resetTimer -= Time.deltaTime;
            //SetCharacterState(EnemyAnimationState.Idle);
        }
    }
    private void FixedUpdate()
    {
        if (isHittingWall() || isNearEdge())
        {
            if (facingDirection == LEFT)
            {
                ChangeFacingDirection(RIGHT);
            }
            else
            {
                ChangeFacingDirection(LEFT);
            }
        }
        if (isDashing == true)
        {
            Dash();
        }
        if (target != null)
        {
            if (target.transform.position.x - gameObject.transform.position.x > 0)
            {
                ChangeFacingDirection(LEFT);
            }
            else
            {
                ChangeFacingDirection(RIGHT);
            }
            if (dashTimer > 0)
            {
                Dash();
                isDashing = true;

            }
            else
            {
                isDashing = false;
                SetCharacterState(EnemyAnimationState.Melee);
                rBody.velocity = Vector2.zero;
            }
        }
        else
        {
            if (dashTimer == startingDashTimer && resetTimer == startingResetTimer)
            {
                rBody.velocity = new Vector2(transform.localScale.x*moveSpeed, rBody.velocity.y);
                SetCharacterState(EnemyAnimationState.Walking);
            }
        }

    }

    private void state_Event(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "footstep")
        {
            transform.GetChild(4).GetComponent<AudioSource>().Play();
        }
        if (e.Data.Name == "slash")
        {
            transform.GetChild(5).GetComponent<AudioSource>().Play();
        }
        if (e.Data.Name == "dash")
        {
            transform.GetChild(6).GetComponent<AudioSource>().Play();
        }

    }
    private void Dash()
    {
        if(!isHittingWall() || !isNearEdge())
        {
            rBody.velocity = new Vector2(transform.localScale.x * dashSpeed, rBody.velocity.y);
            dashTimer -= Time.deltaTime;
            if (currentState != EnemyAnimationState.Melee)
            {
                SetCharacterState(EnemyAnimationState.Dashing);
            }
        }
        else
        {
            SetCharacterState(EnemyAnimationState.Melee);
            rBody.velocity = Vector2.zero;
            dashTimer = 0f;
        }
    }

    bool isHittingWall()
    {
        bool val = false;

        float castDist = baseCastDist;

        if (transform.localScale.x == -1)
        {
            castDist = -baseCastDist;
        }
        Vector3 targetPos = castPos.position;
        targetPos.x += castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.red);

        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Grounded")))
        {
            val = true;
        }
        else
        {
            val = false;
        }


        return val;
    }
    bool isNearEdge()
    {
        bool val = true;

        float castDist = baseCastDist;

        Vector3 targetPos = castPos.position;
        targetPos.y -= castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.blue);

        if (Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Grounded")))
        {
            val = false;
        }
        else
        {
            val = true;
        }


        return val;
    }
    void MeleeAttack()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange,playerLayer);
        if(currentState == EnemyAnimationState.Melee)
        {
            attackTimer -= Time.deltaTime;
        }
        if(hitPlayer != null)
        {
            SetCharacterState(EnemyAnimationState.Melee);
            if (attackTimer <= 0)
            {
                hitPlayer.GetComponent<Player>().Death();
            }
        }
        if (attackTimer <= 0)
        {
            attackTimer = startingAttackTimer;
        }
    }
    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    void ChangeFacingDirection(string newDirection)
    {
        Vector3 newScale = baseScale;
        if (newDirection == LEFT)
        {
            newScale.x = -baseScale.x;
        }
        else
        {
            newScale.x = baseScale.x;
        }
        transform.localScale = newScale;
        facingDirection = newDirection;
    }

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
        if (currentState == EnemyAnimationState.Melee)
        {
            SetCharacterState(EnemyAnimationState.Idle);
        }
    }

    public void SetCharacterState(EnemyAnimationState state)
    {
        currentState = state;


        switch (state)
        {
            case EnemyAnimationState.Walking:
                SetAnimation(walking, true, 0.5f);
                break;
            case EnemyAnimationState.Dashing:
                SetAnimation(dashing, false, 1f);
                break;
            case EnemyAnimationState.Melee:
                SetAnimation(melee, false, 1f);
                break;
            default:
                SetAnimation(idle, true, 1f);
                break;
        }
    }
}


