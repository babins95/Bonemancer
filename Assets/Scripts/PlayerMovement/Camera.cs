using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camera : MonoBehaviour
{
    private CinemachineVirtualCamera thisCamera;
    void Start()
    {
        thisCamera = GetComponent<CinemachineVirtualCamera>();
        //GameManager.instance.camera = thisCamera;
    }
}
