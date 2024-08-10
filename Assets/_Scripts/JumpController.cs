using UnityEngine;

public class JumpController : MonoBehaviour 
{
    [SerializeField] private float _jumpMaxTime;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private Rigidbody _rigidbody;

    private float _timeToReachMaxHeight;
    private float _initialJumpVelocity;
    private float _jumpGravity;
    private float _previousVelocity;
    private float _currentJumpVelocity;
    private float _nextVelocity;
    private float _newVelocity;
    private const float DefaultNegativeYVelocity = -2.0f;

    public bool IsGrounded => _groundChecker.IsGrounded();
    public bool IsGroundedByRaycast => _groundChecker.IsGroundedByRaycast();

    private void Start()
    {
        _timeToReachMaxHeight = _jumpMaxTime / 2;
        _jumpGravity = -2 * _jumpHeight / Mathf.Pow(_timeToReachMaxHeight, 2);
        _initialJumpVelocity = 2 * _jumpHeight / _timeToReachMaxHeight;
    }

    public void ApplyJumpVelocity()
    {
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _currentJumpVelocity, _rigidbody.velocity.z);
        if (IsGroundedByRaycast)
        {
            _currentJumpVelocity = DefaultNegativeYVelocity;
        }
        else
        {
            _previousVelocity = _currentJumpVelocity;
            _nextVelocity = _currentJumpVelocity + (_jumpGravity * Time.fixedDeltaTime);
            _newVelocity = (_previousVelocity + _nextVelocity) * 0.5f;
            _currentJumpVelocity = _newVelocity;
        }
    }

    public void ResetJumpVelocity()
    {
        _currentJumpVelocity = _initialJumpVelocity;
    }
}