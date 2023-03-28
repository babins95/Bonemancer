using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTexts : MonoBehaviour
{
    private Canvas sceneText;
    void Start()
    {
        sceneText = GetComponent<Canvas>();
        GameManager.instance.sceneTexts = sceneText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
