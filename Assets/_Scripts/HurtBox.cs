using UnityEngine;

public class HurtBox : MonoBehaviour
{
    private Health _health;

    public void Initialize(Health health)
    {
        _health = health;
    }

    public void TakeDamage(float damage)
    {
        _health.ApplyDamage(damage);
    }
}