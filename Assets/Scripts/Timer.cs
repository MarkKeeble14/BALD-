using System;
using UnityEngine;

[System.Serializable]
public class Timer
{
    [SerializeField] private float duration;
    private float curTime;
    private bool pause;

    public bool TimesUp => curTime <= 0;
    private Action onExpiry;

    public Timer(float duration, Action onExpiry, bool pauseByDefault = true)
    {
        this.duration = duration;
        this.onExpiry = onExpiry;
        pause = pauseByDefault;
        curTime = duration;
    }

    public void Reset()
    {
        curTime = duration;
    }

    public void Unpause()
    {
        pause = false;
    }

    public void Pause()
    {
        pause = true;
    }

    public void Update()
    {
        if (pause) return;
        curTime -= Time.deltaTime;
        if (curTime < 0)
        {
            onExpiry?.Invoke();
        }
    }
}
