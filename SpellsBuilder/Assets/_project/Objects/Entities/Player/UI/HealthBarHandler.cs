using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarHandler : MonoBehaviour
{
    [SerializeField] private Health healthSource;
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;

    [SerializeField] private Gradient gradient;

    private void Start()
    {
        healthSource.ShowChangesOnClient += ChangeSlider;
        fill.color = gradient.Evaluate(1f);
    }

    private void OnDestroy()
    {
        healthSource.ShowChangesOnClient -= ChangeSlider;
    }

    private void ChangeSlider(float max, float current)
    {
        slider.value = current / max;
        fill.color = gradient.Evaluate(current / max);
    }
}
