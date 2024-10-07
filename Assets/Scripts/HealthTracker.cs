using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthTracker : MonoBehaviour
{
    public Slider HealthBarSlider;
    public Image SliderFill;

    public Material GreenEmission;
    public Material YellowEmission;
    public Material RedEmission;


    private Coroutine _smoothHealthChangeCoroutine;


    public void UpdateSliderValue(float currentHealth, float maxHealth)
    {
        var healthPercentage = Mathf.Clamp01(currentHealth / maxHealth);

        // Update the slider value and size
        // HealthBarSlider.value = healthPercentage;

        // If there is an ongoing smooth health change coroutine, stop it
        if (_smoothHealthChangeCoroutine != null)
        {
            StopCoroutine(_smoothHealthChangeCoroutine);
        }

        // Start a new coroutine for smooth health change
        _smoothHealthChangeCoroutine = StartCoroutine(SmoothHealthChange(HealthBarSlider.value, healthPercentage, 0.5f));

        UpdateColor(healthPercentage);
    }

    private IEnumerator SmoothHealthChange(float startValue, float targetValue, float duration)
    {
        var elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            HealthBarSlider.value = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        HealthBarSlider.value = targetValue;

        _smoothHealthChangeCoroutine = null;
    }

    private void UpdateColor(float healthPercentage)
    {
        SliderFill.material = healthPercentage switch
        {
            >= 0.6f => GreenEmission,
            >= 0.3f => YellowEmission,
            _ => RedEmission
        };
    }

}
