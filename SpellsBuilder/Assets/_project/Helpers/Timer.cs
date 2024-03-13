using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] public Reference<float> interval;
    [SerializeField] public bool autoRestart;
    [SerializeField] private bool active;
    [SerializeField] public UnityEvent elapsed;
    [SerializeField] private bool restartOnEnable;
    private float timeLeft;

    void Start()
    {
        timeLeft = interval;
    }

    void Update()
    {
        if (active)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                elapsed?.Invoke();
                if (autoRestart)
                    Restart();
                else
                    active = false;
            }
        }
    }

    public void Restart()
    {
        timeLeft = interval;
        active = true;
    }

    private void OnEnable()
    {
        if (restartOnEnable)
            Restart();
    }
}
