using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScale : MonoBehaviour
{
    public int fullWidthUnits = 14;
    // Start is called before the first frame update
    void Start()
    {
        float ratio = (float)Screen.height / (float)Screen.width;
        GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = (float)fullWidthUnits * ratio / 2.0f;
    }
}
