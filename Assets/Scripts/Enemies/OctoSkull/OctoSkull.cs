using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class OctoSkullStart : IState
{

    OctoSkull owner;

    public OctoSkullStart(OctoSkull owner)
    {
        this.owner = owner;
    }


    public override void Enter()
    {
        owner.startingTimer = owner.startTimer;
    }

    public override void Execute()
    {
        if (owner.fightStarted == true)
        {
            if(owner.startingTimer == owner.startTimer)
            {
                Debug.Log("Start");
                owner.transform.GetChild(3).gameObject.SetActive(true);
                owner.octoSkullLaugh.Play();
            }
            owner.startTimer -= Time.deltaTime;
            foreach (ChangingAlpha a in owner.alphasToChange)
            {
                a.alphaChange = true;
            }

            if (owner.startTimer <= 0)
            {
                owner.octoSkullStateMachine.SetState(new OctoSkullPause(owner));
            }
        }

    }

    public override void Exit()
    {
        owner.timer = 0;
        owner.fightStarted = false;
    }
}
public class OctoSkullAttack : IState
{

    OctoSkull owner;

    public OctoSkullAttack(OctoSkull owner)
    {
        this.owner = owner;
    }


    public override void Enter()
    {
        owner.timer = owner.attackTimer;
        Debug.Log("Attacco!");
    }

    public override void Execute()
    {
        if (owner.timer < 0)
        {
            foreach (GameObject g in owner.tentacles)
            {
                g.GetComponent<Tentacle>().isAttacking = true;
            }
            owner.octoSkullStateMachine.SetState(new OctoSkullPause(owner));
        }

    }

    public override void Exit()
    {
        owner.timer = 0;
    }
}
public class OctoSkullPause : IState
{

    OctoSkull owner;

    public OctoSkullPause(OctoSkull owner)
    {
        this.owner = owner;
    }


    public override void Enter()
    {
        owner.timer = owner.pauseTimer;
        Debug.Log("Pausa!");
    }

    public override void Execute()
    {
        if (owner.hp >= 3)
        {
            if (owner.timer < 0)
            {
                owner.randomTentacle = owner.tentacles[UnityEngine.Random.Range(0, (owner.tentacles.Length - 1))];
                foreach (GameObject g in owner.tentacles)
                {
                    g.GetComponent<Tentacle>().waiting = false;
                    g.GetComponent<ChangingAlpha>().alphaChange = true;
                }
                owner.randomTentacle.GetComponent<Tentacle>().waiting = true;
                owner.octoSkullStateMachine.SetState(new OctoSkullAttack(owner));
            }
        }
        if(owner.hp == 2)
        {
            if (owner.timer < 0)
            {
                owner.randomTentacle = owner.tentacles[UnityEngine.Random.Range(0, (owner.tentacles.Length - 1))];
                foreach (GameObject g in owner.tentacles)
                {
                    g.GetComponent<Tentacle>().waiting = false;
                    g.GetComponent<ChangingAlpha>().alphaChange = true;
                }
                owner.randomTentacle.GetComponent<Tentacle>().waiting = true;
                owner.randomTentacle.GetComponent<ChangingAlpha>().alphaChange = false;
                owner.octoSkullStateMachine.SetState(new OctoSkullAttack(owner));
            }
        }
        if(owner.hp == 1)
        {
            if (owner.timer < 0)
            {
                owner.randomTentacle = owner.tentacles[UnityEngine.Random.Range(0, (owner.tentacles.Length - 1))];
                foreach (GameObject g in owner.tentacles)
                {
                    g.GetComponent<Tentacle>().waiting = true;
                    g.GetComponent<ChangingAlpha>().alphaChange = false;
                }
                owner.randomTentacle.GetComponent<Tentacle>().waiting = false;
                owner.randomTentacle.GetComponent<ChangingAlpha>().alphaChange = true;
                owner.octoSkullStateMachine.SetState(new OctoSkullAttack(owner));
            }
        }
    }

    public override void Exit()
    {
        owner.timer = 0;
    }
}
public class OctoSkullDeath : IState
{

    OctoSkull owner;

    public OctoSkullDeath(OctoSkull owner)
    {
        this.owner = owner;
    }


    public override void Enter()
    {
        Debug.Log("Death");
        owner.octoSkullDeath.Play();
        owner.timer = 1;
    }

    public override void Execute()
    {

        if(owner.timer < 0)
        {
            owner.transform.GetChild(0).gameObject.transform.position = new Vector2(owner.transform.GetChild(0).gameObject.transform.position.x, owner.transform.GetChild(0).gameObject.transform.position.y - owner.headFallSpeed * Time.deltaTime);
            owner.timer = 0;
        }
        if (owner.timer == 0)
        {
            owner.deathTimer -= Time.deltaTime;
            if (owner.deathTimer <= 0)
            {
                owner.Death();
            }
        }


    }

    public override void Exit()
    {
        
    }
}
public class OctoSkull : MonoBehaviour
{
    public AudioSource octoSkullLaugh;
    public AudioSource octoSkullDeath;

    public static OctoSkull instance;
    public bool fightStarted = false;
    public int hp = 6;
    public float headFallSpeed;
    public GameObject[] tentacles;
    public ChangingAlpha[] alphasToChange;
    public List<GameObject> bgTentacles = new List<GameObject>();
    internal GameObject randomTentacle;
    internal GameObject randomBgTentacle;
    internal float startingTimer;
    public float pauseTimer;
    public float attackTimer;
    public float startTimer;
    internal float deathTimer = 2;
    internal float timer;
    internal bool startingisFinished = false;
    public StateMachine octoSkullStateMachine;
    void Start()
    {
        instance = this;
        octoSkullStateMachine = new StateMachine(new OctoSkullStart(this));
    }
    void Update()
    {
        octoSkullStateMachine.StateUpdate();
        
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        if(hp == 0)
        {
            octoSkullStateMachine.SetState(new OctoSkullDeath(this));
            hp = -1;
        }
    }
    public void Death()
    {
        BossFightStart.instance.ResetCamera();
        if (GameManager.instance != null)
        {
            Destroy(GameManager.instance.gameObject);
            Time.timeScale = 1;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Destroy(gameObject);
    }
    public void BgTentacleDeath()
    {
        if(bgTentacles.Count <= 0)
        {
            return;
        }
        randomBgTentacle = bgTentacles[UnityEngine.Random.Range(0, (bgTentacles.Count - 1))];
        bgTentacles.Remove(randomBgTentacle);
        Destroy(randomBgTentacle);
    }
}
