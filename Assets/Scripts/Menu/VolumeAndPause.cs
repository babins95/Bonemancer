using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeAndPause : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Canvas pauseMenu;
    public Canvas sceneTexts;


    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
    public void Unpause()
    {
        Time.timeScale = 1;
        pauseMenu.gameObject.SetActive(false);
        sceneTexts.gameObject.SetActive(true);
    }
}
