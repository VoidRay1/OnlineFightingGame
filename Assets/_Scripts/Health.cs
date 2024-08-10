using System;

public class Health
{
    private readonly ChampionHealthDisplayer _healthDisplayer;
    private const float MaxHealth = 100.0f;
    private float _currentHealth;

    public event Action OnHealthEnded;

    public Health(ChampionHealthDisplayer healthDisplayer)
    {
        _currentHealth = MaxHealth;
        _healthDisplayer = healthDisplayer;
        _healthDisplayer.InititalizeHealthSlider();
        _healthDisplayer.SetHealthSlidersMaxValue(MaxHealth);
        _healthDisplayer.SetHealthSlidersValue(_currentHealth);
    }

    public void ApplyDamage(float damage)
    {
        if (damage <= 0)
        {
            throw new ArgumentOutOfRangeException($"Damage can't be less or equal 0. Passed damage: {damage}");
        }

        _currentHealth -= damage;
        if (IsHealthEnded())
        {
            OnHealthEnded?.Invoke();
        }

        _healthDisplayer.UpdateHealthSlider(_currentHealth);
    }

    public void ResetHealthToMax()
    {
        _currentHealth = MaxHealth;
        _healthDisplayer.ResetSlidersValues();
    }

    private bool IsHealthEnded()
    {
        return _currentHealth <= 0;
    }
}