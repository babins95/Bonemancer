using System.Collections;
using UnityEngine;
using Spine.Unity;
using Spine;
using System;

public enum QueenAnimationState { Idle, Walking, Dashing, FirstSlash, SecondSlash,ThirdSlash, Mortar, Bullet, Death }
public class QueenStart : IState
{

    QueenStateMachine owner;

    public QueenStart(QueenStateMachine owner)
    {
        this.owner = owner;
        owner.SetCharacterState(QueenAnimationState.Idle);
    }


    public override void Enter()
    {

    }

    public override void Execute()
    {
        if (owner.fightStarted == true)
        {
            owner.SetCharacterState(QueenAnimationState.Walking);
            SpineController.instance.gameObject.GetComponent<SpineController>().enabled = false;
            SpineController.instance.gameObject.transform.localScale = new Vector2(1f, 1f);
            SpineController.instance.SetAnimation(SpineController.instance.idle, true, 1f);
            owner.queenRb.velocity = Vector2.left * Time.deltaTime * owner.walkingSpeed;
            owner.walkingTimer -= Time.deltaTime;
            if (owner.walkingTimer <= 0)
            {
                SpineController.instance.gameObject.GetComponent<SpineController>().enabled = true;
                owner.ChangeState();
            }

        }
    }

    public override void Exit()
    {

    }
}
public class QueenPause : IState
{

    QueenStateMachine owner;

    public QueenPause(QueenStateMachine owner)
    {
        this.owner = owner;
        owner.SetCharacterState(QueenAnimationState.Idle);
    }


    public override void Enter()
    {
        owner.pauseTimer = owner.pauseBetweenAttack;
    }

    public override void Execute()
    {
        if (owner.transform.position.x - Player.instance.transform.position.x > 1)
        {
            owner.transform.localScale = new Vector2(owner.startingScaleX, owner.transform.localScale.y);
        }
        else
        {
            owner.transform.localScale = new Vector2(-owner.startingScaleX, owner.transform.localScale.y);
        }
        owner.pauseTimer -= Time.deltaTime;
        if (owner.pauseTimer <= 0)
        {
            owner.pauseTimer = owner.pauseBetweenAttack;
            owner.ChangeState();
        }
    }

    public override void Exit()
    {

    }
}
public class QueenDash : IState
{

    QueenStateMachine owner;

    public QueenDash(QueenStateMachine owner)
    {
        this.owner = owner;
    }


    public override void Enter()
    {
        owner.dashDestination = Player.instance.transform.position.x;
        owner.SetCharacterState(QueenAnimationState.Dashing);
        if (owner.transform.position.x - Player.instance.transform.position.x > 1)
        {
            owner.transform.localScale = new Vector2(owner.startingScaleX, owner.transform.localScale.y);
        }
        else
        {
            owner.transform.localScale = new Vector2(-owner.startingScaleX, owner.transform.localScale.y);
        }
    }

    public override void Execute()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(owner.attackPoint.position, owner.attackRange, owner.playerLayer);
        if (owner.transform.position.x - Player.instance.transform.position.x > 8 || owner.transform.position.x - Player.instance.transform.position.x < -8)
        {
            owner.queenRb.velocity = new Vector2(owner.transform.localScale.x * -owner.dashSpeed, owner.queenRb.velocity.y);
        }
        else
        {
            int stateNumber = UnityEngine.Random.Range(1, 3);
            if(stateNumber == 1)
            {
                owner.stateMachine.SetState(new QueenDoubleSlash(owner));
            }
            else
            {
                owner.stateMachine.SetState(new QueenTripleSlash(owner));
            }
        }
    }

    public override void Exit()
    {

    }
}
public class QueenDoubleSlash : IState
{

    QueenStateMachine owner;

    public QueenDoubleSlash(QueenStateMachine owner)
    {
        this.owner = owner;
    }


    public override void Enter()
    {
        Debug.Log("Doppio Slash!");
        owner.attackTimer = 0;
    }

    public override void Execute()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(owner.attackPoint.position, owner.attackRange, owner.playerLayer);
        owner.attackTimer += Time.deltaTime;
        if (owner.secondAttack == false)
        {
            if (owner.attackTimer >= owner.firstAttackTimer - owner.stepBeforeAttack)
            {
                owner.queenRb.velocity = new Vector2(owner.transform.localScale.x * owner.stepSpeed, owner.queenRb.velocity.y);
            }
            owner.SetCharacterState(QueenAnimationState.FirstSlash);
            if (hitPlayer != null)
            {
                if (owner.attackTimer >= owner.firstAttackTimer)
                {
                    hitPlayer.GetComponent<Player>().Death();
                    owner.stateMachine.SetState(new QueenPause(owner));
                }
            }
            if (owner.attackTimer >= owner.firstAttackTimer)
            {
                owner.attackTimer = 0;
                owner.secondAttack = true;
            }
        }
        else if (owner.secondAttack == true)
        {
            if (owner.attackTimer >= owner.secondAttackTimer - owner.stepBeforeAttack)
            {
                owner.queenRb.velocity = new Vector2(owner.transform.localScale.x * owner.stepSpeed, owner.queenRb.velocity.y);
            }
            owner.SetCharacterState(QueenAnimationState.SecondSlash);
            if (hitPlayer != null)
            {
                if (owner.attackTimer >= owner.secondAttackTimer)
                {
                    hitPlayer.GetComponent<Player>().Death();
                    owner.stateMachine.SetState(new QueenPause(owner));
                }
            }
            if (owner.attackTimer >= owner.secondAttackTimer)
            {
                owner.attackTimer = 0;
                owner.stateMachine.SetState(new QueenPause(owner));
            }
        }
    }

    public override void Exit()
    {
        owner.secondAttack = false;
    }
}
public class QueenTripleSlash : IState
{

    QueenStateMachine owner;

    public QueenTripleSlash(QueenStateMachine owner)
    {
        this.owner = owner;
    }


    public override void Enter()
    {
        Debug.Log("Triplo Slash!");
        owner.attackTimer = 0;
    }

    public override void Execute()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(owner.attackPoint.position, owner.attackRange, owner.playerLayer);
        owner.attackTimer += Time.deltaTime;
        if (owner.secondAttack == false && owner.thirdAttack == false)
        {
            if (owner.attackTimer >= owner.firstAttackTimer - owner.stepBeforeAttack)
            {
                owner.queenRb.velocity = new Vector2(owner.transform.localScale.x * owner.stepSpeed, owner.queenRb.velocity.y);
            }
            owner.SetCharacterState(QueenAnimationState.FirstSlash);
            if (hitPlayer != null)
            {
                if (owner.attackTimer >= owner.firstAttackTimer)
                {
                    hitPlayer.GetComponent<Player>().Death();
                    owner.stateMachine.SetState(new QueenPause(owner));
                }
            }
            if (owner.attackTimer >= owner.firstAttackTimer)
            {
                owner.attackTimer = 0;
                owner.secondAttack = true;
            }
        }
        if (owner.secondAttack == true && owner.thirdAttack == false)
        {
            if (owner.attackTimer >= owner.secondAttackTimer - owner.stepBeforeAttack)
            {
                owner.queenRb.velocity = new Vector2(owner.transform.localScale.x * owner.stepSpeed, owner.queenRb.velocity.y);
            }
            owner.SetCharacterState(QueenAnimationState.SecondSlash);
            if (hitPlayer != null)
            {
                if (owner.attackTimer >= owner.secondAttackTimer)
                {
                    hitPlayer.GetComponent<Player>().Death();
                    owner.stateMachine.SetState(new QueenPause(owner));
                }
            }
            if (owner.attackTimer >= owner.secondAttackTimer)
            {
                owner.attackTimer = 0;
                owner.thirdAttack = true;
            }
        }
        if (owner.thirdAttack == true)
        {
            if (owner.attackTimer >= owner.thirdAttackTimer - owner.stepBeforeAttack)
            {
                owner.queenRb.velocity = new Vector2(owner.transform.localScale.x * owner.stepSpeed, owner.queenRb.velocity.y);
            }
            owner.SetCharacterState(QueenAnimationState.ThirdSlash);
            if (hitPlayer != null)
            {
                if (owner.attackTimer >= owner.thirdAttackTimer)
                {
                    hitPlayer.GetComponent<Player>().Death();
                    owner.stateMachine.SetState(new QueenPause(owner));
                }
            }
            if (owner.attackTimer >= owner.thirdAttackTimer)
            {
                owner.attackTimer = 0;
                owner.stateMachine.SetState(new QueenPause(owner));
            }
        }
    }

    public override void Exit()
    {
        owner.secondAttack = false;
        owner.thirdAttack = false;
    }
}
public class QueenShooting : IState
{

    QueenStateMachine owner;

    public QueenShooting(QueenStateMachine owner)
    {
        this.owner = owner;
        owner.SetCharacterState(QueenAnimationState.Bullet);
    }


    public override void Enter()
    {
        Debug.Log("Proiettile");
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {

    }
}
public class QueenMortar : IState
{

    QueenStateMachine owner;

    public QueenMortar(QueenStateMachine owner)
    {
        this.owner = owner;
        owner.gameObject.transform.GetChild(1).GetComponent<AudioSource>().Play();
        owner.SetCharacterState(QueenAnimationState.Mortar);
    }


    public override void Enter()
    {
        Debug.Log("Mortaio!");
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }
}
public class QueenStateMachine : MonoBehaviour
{
    static public QueenStateMachine instance;
    internal Rigidbody2D queenRb;
    public StateMachine stateMachine;
    public QueenBullet queenBulletPrefab;
    public QueenMortarShoot queenMortarPrefab;
    public MovingPlatform movingPlatform;

    public AnimationReferenceAsset idle, walking, dashing, firstSlash, secondSlash, thirdSlash, death, mortar, bullet;
    public SkeletonAnimation skeletonAnimation;
    public QueenAnimationState currentState;
    public string currentAnimation;

    public float pauseBetweenAttack;
    internal float pauseTimer;

    public bool fightStarted = false;
    public float walkingSpeed;
    public float walkingTimer = 2f;

    public int hp = 3;
    public float invulnerabilityTime = 2f;
    bool invulnerable = false;

    internal bool bulletToSpawn = false;

    internal float flashing = 0.2f;

    internal bool secondAttack = false;
    internal bool thirdAttack = false;
    public float firstAttackTimer = 0.2f;
    public float secondAttackTimer = 0.2f;
    public float thirdAttackTimer = 0.2f;
    internal float attackTimer;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;
    public Transform attackPoint;
    public float stepBeforeAttack;
    public float stepSpeed;
    internal float startingScaleX;

    public float dashSpeed;
    internal float dashDestination = 0f;

    internal void ChangeState()
    {
        int stateNumber = UnityEngine.Random.Range(1, 4);
        switch (stateNumber)
        {
            case 1:
                stateMachine.SetState(new QueenDash(this));
                break;
            case 2:
                stateMachine.SetState(new QueenMortar(this));
                break;
            case 3:
                stateMachine.SetState(new QueenShooting(this));
                break;
            default:
                break;
        }
    }

    void Start()
    {
        startingScaleX = transform.localScale.x;
        instance = this;
        queenRb = GetComponent<Rigidbody2D>();
        skeletonAnimation.state.Event += state_Event;
        stateMachine = new StateMachine(new QueenStart(this));
    }

    private void state_Event(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "queenspit")
        {
            ShootMortar();
        }
        if(e.Data.Name == "queenshot")
        {
            gameObject.transform.GetChild(0).GetComponent<AudioSource>().Play();
            if (bulletToSpawn == true)
            {
                ShootBullet(gameObject.transform.GetChild(0).position);
                bulletToSpawn = false;
            }
            else if (bulletToSpawn == false)
            {
                Debug.Log("secondo sparo");
                ShootBullet(gameObject.transform.GetChild(4).position);
                bulletToSpawn = true;
            }
        }
        if (e.Data.Name == "footstep")
        {
            gameObject.transform.GetChild(7).GetComponent<AudioSource>().Play();
        }
        if (e.Data.Name == "dash")
        {
            gameObject.transform.GetChild(6).GetComponent<AudioSource>().Play();
        }
        if (e.Data.Name == "slash")
        {
            gameObject.transform.GetChild(5).GetComponent<AudioSource>().Play();
        }
    }

    private void Update()
    {
        stateMachine.StateUpdate();
        if(invulnerable == true)
        {
            StartCoroutine("Invulnerability");
        }
    }
    public void TakeDamage()
    {
        if (!invulnerable)
        {
            Player.instance.AttackSound();
            gameObject.transform.GetChild(4).GetComponent<AudioSource>().Play();
            pauseTimer = pauseBetweenAttack - 1f;
            hp--;
            if (hp <= 0)
            {
                BossFightStart.instance.ResetCamera();
                SetCharacterState(QueenAnimationState.Death);
                foreach(Transform child in transform)
                {
                    Destroy(child.gameObject);
                }
                movingPlatform.enabled = true;
                this.enabled = false;

            }

            invulnerable = true;
        }

    }


    IEnumerator Invulnerability()
    {
        flashing += Time.deltaTime;
        if(flashing >= 0.05)
        {
            if (gameObject.GetComponent<MeshRenderer>().enabled == true)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
            flashing = 0;
        }
        yield return new WaitForSeconds(invulnerabilityTime);
        if (gameObject.GetComponent<MeshRenderer>().enabled == false)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        invulnerable = false;
    }
    IEnumerator Pause()
    {
        yield return new WaitForSeconds(pauseBetweenAttack);
        ChangeState();
    }
    public void ShootBullet(Vector2 spawnPoint)
    {
        if (SpineController.instance.transform.position.x - transform.position.x < 0)
        {
            queenBulletPrefab.GetComponent<QueenBullet>().bulletAngle = Vector2.left;
        }
        else
        {
            queenBulletPrefab.GetComponent<QueenBullet>().bulletAngle = Vector2.right;
        }
        QueenBullet newBullet = Instantiate(queenBulletPrefab);
        newBullet.transform.position = spawnPoint;
        if (bulletToSpawn == true)
        {
            stateMachine.SetState(new QueenPause(this));
        }
    }
    public void ShootMortar()
    {
        QueenMortarShoot newMortar = Instantiate(queenMortarPrefab);
        newMortar.transform.position = gameObject.transform.GetChild(1).position;
        stateMachine.SetState(new QueenPause(this));
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(currentAnimation))
        {
            return;
        }
        TrackEntry animationEntry = skeletonAnimation.state.SetAnimation(0, animation, loop);
        animationEntry.TimeScale += timeScale;
        animationEntry.Complete += AnimationEntry_Complete;
        currentAnimation = animation.name;
    }

    private void AnimationEntry_Complete(Spine.TrackEntry trackEntry)
    {
        if (currentState == QueenAnimationState.Mortar)
        {
            //stateMachine.SetState(new QueenPause(this));
        }
    }

    public void SetCharacterState(QueenAnimationState state)
    {
        currentState = state;


        switch (state)
        {
            case QueenAnimationState.Walking:
                SetAnimation(walking, true, 1f);
                break;
            case QueenAnimationState.Dashing:
                SetAnimation(dashing, false, 1f);
                break;
            case QueenAnimationState.FirstSlash:
                SetAnimation(firstSlash, false, 0.2f);
                break;
            case QueenAnimationState.SecondSlash:
                SetAnimation(secondSlash, false, 1f);
                break;
            case QueenAnimationState.ThirdSlash:
                SetAnimation(thirdSlash, false, 1f);
                break;
            case QueenAnimationState.Death:
                SetAnimation(death, false, 1f);
                break;
            case QueenAnimationState.Mortar:
                SetAnimation(mortar, false, 1f);
                break;
            case QueenAnimationState.Bullet:
                SetAnimation(bullet, false, 1f);
                break;
            default:
                SetAnimation(idle, true, 0.2f);
                break;
        }
    }

}
