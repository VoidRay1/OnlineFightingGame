using UnityEngine;

public class ChampionTracker : MonoBehaviour
{
    [SerializeField] private Transform _championHead;
    private Transform _enemyChampionHead;
    private bool _isEnabled;
    private const float MinHeadRotation = -35.0f;
    private const float MaxHeadRotation = 35.0f;

    public Transform ChampionHead => _championHead;

    public void Initialize(Transform enemyChampionHead)
    {
        _enemyChampionHead = enemyChampionHead;
    }

    public void SelfLateUpdate()
    {
        if(_isEnabled && _enemyChampionHead is not null)
        {
            _championHead.LookAt(_enemyChampionHead);
/*            _championHead.eulerAngles = new Vector3
            (
                Mathf.Clamp(_championHead.eulerAngles.x, MinHeadRotation, MaxHeadRotation),
                _championHead.eulerAngles.y,
                _championHead.eulerAngles.z
            );*/
        }
    }

    public void Enable()
    {
        _isEnabled = true;
    }

    public void Disable()
    {
        _isEnabled = false;
    }
}