using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FPSCounter : MonoBehaviour
{
    float oneSecond = 1f;
    TMP_Text fpsCounter;
    float timeElapsed = 0f;
    int frames = 0;
    private void Awake()
    {
        fpsCounter = GetComponent<TMP_Text>();
    }

    void Update()
    {
        frames++;
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= oneSecond)
        {
            fpsCounter.text = frames.ToString();
            frames = 0;
            timeElapsed = 0f;
        }
    }
}
