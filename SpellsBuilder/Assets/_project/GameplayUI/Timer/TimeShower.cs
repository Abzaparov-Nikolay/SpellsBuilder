using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeShower : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI time;

    void Start()
    {
        StartCoroutine(DisplayTime());
    }

    public IEnumerator DisplayTime()
    {
        while (true)
        {
            int minutes = Mathf.FloorToInt(LevelTimer.Value / 60F);
            int seconds = Mathf.FloorToInt(LevelTimer.Value - minutes * 60);

            string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
            time.text = niceTime;
            yield return new WaitForSeconds(1);
        }
    }
}
