using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangingPlayer : MonoBehaviour
{
    public GameObject spinePlayerPrefab;
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;
    public ParticleSystem smoke;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HeadController head = collision.GetComponent<HeadController>();
        if(head != null)
        {
            GetComponent<AudioSource>().Play();
            Destroy(head.gameObject);
            StartCoroutine("Changing");
        }
    }
    IEnumerator Changing()
    {
        smoke.Play();
        ParticleSystem.EmissionModule em = smoke.emission;
        em.enabled = true;
        yield return new WaitForSeconds(2f);
        spinePlayerPrefab.gameObject.SetActive(true);
        camera1.Priority = 0;
        camera2.Priority = 1;
        em.enabled = false;
        Destroy(gameObject);
    }
}
