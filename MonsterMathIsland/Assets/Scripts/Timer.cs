using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public float timeInSecs = 10;
    public TMP_Text timerText;
    public UnityEvent onTimeElapsed;
    public bool startOnAwake = true;

    private bool isTimerOn;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = timeInSecs;
        if (startOnAwake) StartTimer();

    }

    public void StartTimer()
    {
        isTimerOn = true;
    }

    public void StopTimer()
    {
        isTimerOn = false;
        timer = timeInSecs;
    }

    public void PauseTimer()
    {
        isTimerOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerOn)
        {
            timer -= Time.deltaTime;
            timerText.text = timer.ToString("0.00");
            if(timer <= 0)
            {
                onTimeElapsed.Invoke();
                timer = timeInSecs;
            }
        }
    }
}
