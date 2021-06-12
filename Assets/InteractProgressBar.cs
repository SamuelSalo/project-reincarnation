using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractProgressBar : MonoBehaviour
{
    private bool running;
    private float duration;
    private float timer;
    private Slider slider;

    public void StartProgress(float _duration)
    {
        if (running) return;
        if (!slider) slider = transform.GetChild(0).GetComponent<Slider>();

        duration = _duration;
        running = true;
        slider.maxValue = duration;
        slider.gameObject.SetActive(true);
    }

    public void StopProgress()
    {
        if (!running) return;
        if (!slider) slider = transform.GetChild(0).GetComponent<Slider>();

        duration = 0f;
        timer = 0f;
        running = false;
        slider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(running)
        {
            timer += Time.deltaTime;
            slider.value = timer;

            if (timer >= duration)
                StopProgress();
        }
    }
}
