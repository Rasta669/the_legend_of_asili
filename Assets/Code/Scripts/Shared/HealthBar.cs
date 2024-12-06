using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Gradient _healthGradient;
    [SerializeField] private Image _fill;

    public void SetSlider(float amount)
    {
        _healthSlider.value = amount;
        _fill.color = _healthGradient.Evaluate(_healthSlider.normalizedValue);
    }
    public void SetSliderMax(float amount)
    {
        _healthSlider.maxValue = amount;
        _fill.color = _healthGradient.Evaluate(1f);
        SetSlider(amount);
    }
}
