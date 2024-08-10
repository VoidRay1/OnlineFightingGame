using UnityEngine;

public class EaseLerpTest : MonoBehaviour
{
    [SerializeField] private float _time;
    [SerializeField] private Vector3 _offset;

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private float _normalizedTime;

    private void Start()
    {
        _startPosition = transform.position;
        _endPosition = transform.position + _offset;
        _normalizedTime = 0.0f;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        transform.position = Vector3.Lerp(_startPosition, _endPosition, EaseInQuint(_normalizedTime));
        _normalizedTime += Time.deltaTime / _time;
    }

    private float EaseInQuint(float x)
    {
        return x * x * x * x * x;
    }
}