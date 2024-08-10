using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    private Camera _camera;
    private Transform _firstChampionTransform;
    private Transform _secondChampionTransform;
    private float _cachedDistanceBetweenChampions;
    private float _moveStartTime;
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private float _lengthDistance;
    private bool _isMoving;
    private const float MaxPossibleFieldOfView = 70.0f;
    private const float MinPossibleFieldOfView = 65.0f;

    public void Initialize(Transform firstChampionTransform, Transform secondChampionTransform)
    {
        _firstChampionTransform = firstChampionTransform;
        _secondChampionTransform = secondChampionTransform;
    }

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        if(_firstChampionTransform == null || _secondChampionTransform == null)
        {
            return;
        }
        //_cachedDistanceBetweenChampions = Vector3.Distance(_firstChampionTransform.localPosition, _secondChampionTransform.localPosition);
        _startPosition = _camera.transform.position;
        _targetPosition = new Vector3(GetCenterPointBetweenChampions().x, _camera.transform.position.y, _camera.transform.position.z);
        _lengthDistance = Vector3.Distance(_camera.transform.position, _targetPosition);
        if (_isMoving == false)
        {
            _moveStartTime = Time.time;
        }

        if (_camera.transform.position != _targetPosition)
        {
            Move(_targetPosition);
        }
        else
        {
            _isMoving = false;
        }
    }

    private void Move(Vector3 targetPosition)
    {
        _isMoving = true;
        float distanceCovered = (Time.time - _moveStartTime) * _moveSpeed;
        float fractionOfJourney = distanceCovered / _lengthDistance;
/*        print("Distance covered: " + distanceCovered);
        print("Length distance: " + _lengthDistance);
        print("Fraction: " + fractionOfJourney);*/
        _camera.transform.position = Vector3.Lerp(_startPosition, targetPosition, fractionOfJourney);
    }

    private Vector3 GetCenterPointBetweenChampions()
    {
        return Vector3.Lerp(_firstChampionTransform.localPosition, _secondChampionTransform.localPosition, 0.5f);
    }
  
    private bool IsDistanceBetweenChampionsChanged()
    {
        return Mathf.Approximately(Vector3.Distance(_firstChampionTransform.localPosition, _secondChampionTransform.localPosition), _cachedDistanceBetweenChampions) == false;
    }
}