using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack Move/Moves List")]
public class MovesList : ScriptableObject
{
    [SerializeField] private List<AttackMoveData> _moves;

    public IReadOnlyList<AttackMoveData> Moves => _moves;
}