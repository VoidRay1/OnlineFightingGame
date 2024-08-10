using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack Move/Move Properties")]
public class MoveProperties : ScriptableObject
{
    [SerializeField] private MoveType _moveType;
    [SerializeField, Range(MinDamage, MaxDamage)] private float _damage = MinDamage;
    private float _blockDamage;

    private const float MinDamage = 2.0f;
    private const float MaxDamage = 14.0f;
    private const byte BlockDamageDecreaseFactor = 5;
    private const byte DigitsToRound = 2;

    public MoveType MoveType => _moveType;
    public float Damage => _damage;
    public float BlockDamage => _blockDamage;

    private void Awake()
    {
        _blockDamage = RecalculateBlockDamage();
    }

    private void OnValidate()
    {
        _blockDamage = RecalculateBlockDamage();
    }

    private float RecalculateBlockDamage()
    {
        return (float)Math.Round(_damage * (100 / BlockDamageDecreaseFactor) / 100, DigitsToRound);
    }
}