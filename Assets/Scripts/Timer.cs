using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private int _remainingSeconds;

    private Coroutine _tick;
    public Action OnFinish;

    public void Time(int seconds)
    {
        Stop();
        
        _remainingSeconds = seconds;
        _tick = StartCoroutine(Tick());
    }
    
    public void Stop()
    {
        if (_tick == null) return;
        StopCoroutine(_tick);
        _remainingSeconds = 0;
        _tick = null;
    }

    public void Pause()
    {
        StopCoroutine(_tick);
        _tick = null;
    }

    public void Resume()
    {
        _tick = StartCoroutine(Tick());
    }

    private IEnumerator Tick()
    {
        while (_remainingSeconds > 0)
        {
            yield return new WaitForSeconds(1);
            _remainingSeconds--;
            
            timerText.text = FormatToTimer(_remainingSeconds);
        }
        OnFinish?.Invoke();
    }

    private string FormatToTimer(int seconds)
    {
        int minutes = seconds / 60;
        int secs = seconds % 60;
        return $"{minutes}:{secs:D2}";
    }
}
