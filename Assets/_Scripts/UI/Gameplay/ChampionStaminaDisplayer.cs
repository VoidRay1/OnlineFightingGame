using UnityEngine;
using UnityEngine.UI;

public class ChampionStaminaDisplayer : MonoBehaviour
{
    [SerializeField] private Slider _staminaSlider;

    public void SetStaminaSliderMaxValue(float maxValue)
    {
        _staminaSlider.maxValue = maxValue;
    }

    public void SetStaminaSliderValue(float value)
    {
        _staminaSlider.value = value;
    }
}