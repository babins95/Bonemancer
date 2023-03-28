using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private Canvas thisPauseMenu;
    void Start()
    {
        thisPauseMenu = GetComponent<Canvas>();
        GameManager.instance.pauseMenu = thisPauseMenu;
        gameObject.SetActive(false);
    }
}
