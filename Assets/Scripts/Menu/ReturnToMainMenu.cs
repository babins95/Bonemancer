using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    float timer = 0;
    float tempoAttesa = 1;
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.anyKey && timer >= tempoAttesa)
        {
            SceneManager.LoadScene(0);
        }
    }
}
