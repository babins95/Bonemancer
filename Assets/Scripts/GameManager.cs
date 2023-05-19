using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Canvas pauseMenu;
    public Canvas sceneTexts;
    //public GameObject headPrefab;
    //public Transform spawnPoint;
    //public CinemachineVirtualCamera camera;
    //public GameObject player;
    static public GameManager instance;

    [SerializeField] CheckPoint lastCheckPoint;

    public Vector3 lastCheckPointPos { get; private set; }

    private void Awake()
    {
        lastCheckPointPos = lastCheckPoint.transform.position;
        //if (!player)
        //{
        //    player = Instantiate(headPrefab, lastCheckPointPos, Quaternion.identity);
        //}
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator DelayedReload()
    {

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    public void SetCheckpoint(CheckPoint last)
    {
        lastCheckPoint = last;
        lastCheckPointPos = lastCheckPoint.transform.position;
    }
    public void Reload()
    {
        StartCoroutine("DelayedReload");
    }
    //void Start()
    //{
    //    player = Instantiate(headPrefab, lastCheckPointPos, Quaternion.identity);
    //    camera.Follow = player.transform;
    //}

    void Update()
    {
        //camera.Follow = player.transform;
        ///PlayerDeath();
        //camera.Follow = player.transform;
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }

    //private void PlayerDeath()
    //{
    //    if(!player)
    //    {
    //        Reload();
    //    }
    //}

    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            pauseMenu.gameObject.SetActive(true);
            sceneTexts.gameObject.SetActive(false);
        }
    }
    public void Unpause()
    {
        Time.timeScale = 1;
        pauseMenu.gameObject.SetActive(false);
        sceneTexts.gameObject.SetActive(true);
    }

}
