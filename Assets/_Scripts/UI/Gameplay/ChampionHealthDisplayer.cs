using UnityEngine;

public class ChampionHealthDisplayer : MonoBehaviour
{
    [SerializeField] private AppliedDamageSliderDisplayer _appliedDamageDisplayer;
    [SerializeField] private HealthSliderDisplayer _healthSliderDisplayer;

    public void InititalizeHealthSlider()
    {
        _healthSliderDisplayer.Initialize();
    }

    public void SetHealthSlidersMaxValue(float maxValue)
    {
        _healthSliderDisplayer.SetHealthSliderMaxValue(maxValue);
        _appliedDamageDisplayer.SetAppliedDamageSliderMaxValue(maxValue);
    }

    public void SetHealthSlidersValue(float value) 
    {
        _healthSliderDisplayer.SetHealthSliderValue(value);
        _appliedDamageDisplayer.SetAppliedDamageSliderValue(value);
    }

    public void UpdateHealthSlider(float health)
    {
        _healthSliderDisplayer.SetHealthSliderValue(health);
        _appliedDamageDisplayer.Show(health);
    }

    public void ResetSlidersValues()
    {
        _healthSliderDisplayer.Reset();
        _appliedDamageDisplayer.Reset();
    }
}