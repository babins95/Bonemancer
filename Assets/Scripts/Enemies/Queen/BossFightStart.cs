using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossFightStart : MonoBehaviour
{
    public bool isShaking = false;
    public float shakeTimer = 1f;
    private float startingShakeTimer;
    public CinemachineVirtualCamera cam1;
    public CinemachineVirtualCamera camQueen;
    static public BossFightStart instance;
    private void Start()
    {
        instance = this;
        startingShakeTimer = shakeTimer;
    }
    private void Update()
    {
        if(isShaking == true)
        {
            ShakeCamera(5f);
            shakeTimer -= Time.deltaTime;
            if(shakeTimer <= 0)
            {
                isShaking = false;
                shakeTimer = startingShakeTimer;
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = camQueen.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ParallaxEffect.instance.bossRoom = true;
        if(collision.gameObject.GetComponent<Player>())
        {
            Player.instance.isInBossFight = true;
            transform.position = Vector3.zero;
            cam1.Priority = 0;
            camQueen.Priority = 1;
            if (QueenStateMachine.instance != null)
            {
                QueenStateMachine.instance.fightStarted = true;
            }
            if (OctoSkull.instance != null)
            {
                OctoSkull.instance.fightStarted = true;
            }
        }
    }
    public void ResetCamera()
    {
        cam1.Priority = 1;
        camQueen.Priority = 0;
        ParallaxEffect.instance.bossRoom = false;
        Player.instance.isInBossFight = false;
        Destroy(gameObject);
    }
    public void ShakeCamera(float intensity)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = camQueen.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

    }
}
