using System.Collections.Generic;
using UnityEngine;

public class ChampionSpawnPoint : MonoBehaviour
{
    [SerializeField] private Transform _spawnTransform;
    [SerializeField] private ViewDirection _viewDirection;
    private readonly Dictionary<ViewDirection, Vector3> _viewRotations = new Dictionary<ViewDirection, Vector3>()
    {
        [ViewDirection.Right] = new Vector3(0.0f, 90.0f, 0.0f),
        [ViewDirection.Left] = new Vector3(0.0f, 270.0f, 0.0f),
    };
    private Vector3 _viewRotation;

    public Vector3 Position => _spawnTransform.position;
    public ViewDirection ViewDirection => _viewDirection;

    private void Awake()
    {
        SetViewRotation();
    }

    private void SetViewRotation()
    {
        switch (_viewDirection)
        {
            case ViewDirection.Right:
                _viewRotation = _viewRotations[ViewDirection.Right];
                break;
            case ViewDirection.Left:
                _viewRotation = _viewRotations[ViewDirection.Left];
                break;
        }
    }

    public Quaternion GetViewRotation()
    {
        return Quaternion.Euler(_viewRotation);
    }
}