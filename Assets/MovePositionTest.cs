using UnityEngine;

public class MovePositionTest : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private bool _isMovingInFixedUpdate = true;

    private void Update()
    {
        if (_isMovingInFixedUpdate == false)
        {
            _rigidbody.MovePosition(transform.position + Vector3.right * Time.deltaTime * _speed);
        }
    }

    private void FixedUpdate()
    {
        if(_isMovingInFixedUpdate)
        {
            _rigidbody.MovePosition(transform.position + Vector3.right * Time.deltaTime * _speed);
        }
    }
}