using UnityEngine;

public class MoveController : MonoBehaviour
{
    [SerializeField] private AnimationCurve _stepAnimationCurve;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField, Range(1.0f, 100.0f)] private float _moveSpeed;
    [SerializeField, Range(1.25f, 10.0f)] private float _runSpeedMultiplier;
    [SerializeField] private float _minStepDistance = 0.3f;
    [SerializeField] private float _time;

    private float _stepTime;
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private float _normalizedTime;

    public bool IsStepCompleted => transform.position == _targetPosition && _normalizedTime > 1.0f;

    [ContextMenu("Build Curve")]
    public void BuildCurve()
    {
        _stepAnimationCurve.keys = null;
        float time = 0.0f;
        while (Mathf.Approximately(time, 0.54f) == false)
        {
            _stepAnimationCurve.AddKey(time, 0.0f);
            time += 0.005f;
        }
        time = 0.85f;
        while (Mathf.Approximately(time, 1.0f) == false)
        {
            _stepAnimationCurve.AddKey(time, 1.0f);
            time += 0.005f;
        }
    }

    private void Start()
    {
        _startPosition = transform.position;
        _targetPosition = transform.position;
    }

    public void InitializeStepDirection(Vector3 direction, float animationLength)
    {
        _stepTime = animationLength;
        _normalizedTime = 0.0f;
        Vector3 targetPosition = transform.position;
        _startPosition = transform.position;

        if (direction == Vector3.left)
        {
            targetPosition = new Vector3(transform.position.x - _minStepDistance, transform.position.y, transform.position.z);
        }
        else if (direction == Vector3.right)
        {
            targetPosition = new Vector3(transform.position.x + _minStepDistance, transform.position.y, transform.position.z);
        }
        _targetPosition = targetPosition;
    }

    public void Step()
    {
        _rigidbody.MovePosition(Vector3.Lerp(_startPosition, _targetPosition, _stepAnimationCurve.Evaluate(_normalizedTime)));
        _normalizedTime += Time.deltaTime / _stepTime;
    }

    public void Move(Vector3 direction)
    {
        _rigidbody.MovePosition(transform.position + new Vector3(direction.x, 0.0f, 0.0f) * Time.deltaTime * _moveSpeed);
    }

    public void Run(Vector3 direction)
    {
        _rigidbody.MovePosition(transform.position + new Vector3(direction.x, 0.0f, 0.0f) * Time.deltaTime * _moveSpeed * _runSpeedMultiplier);
    }

    private float EaseInOutCubic(float x)
    {
        return x > 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
    }
}