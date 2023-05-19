using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    private float _elapsedTime;

    public void ResetTime() => _elapsedTime = 0f;
    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        if(_elapsedTime >= 5f)
        {
            gameObject.SetActive(false);
        }
    }
}
