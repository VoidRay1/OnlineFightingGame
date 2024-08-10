using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private OverlapGameObject _overlapGameObject;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField, Range(0.1f, 50.0f)] private float _rayDistance;
    [Header("Box parameters")]
    [SerializeField] private Vector3 _boxCenterOffset;
    [SerializeField] private Vector3 _boxSize;
    [Header("Capsule parameters")]
    [SerializeField] private Vector3 _capsuleFirstPoint;
    [SerializeField] private Vector3 _capsuleSecondPoint;
    [SerializeField] private float _capsuleHeight;
    [SerializeField] private float _capsuleRadius;
    [Header("Sphere parameters")]
    [SerializeField] private Vector3 _sphereCenterOffset;
    [SerializeField] private float _sphereRadius;
    private const byte CollidersBufferSize = 1;
    private readonly Collider[] _collidersBuffer = new Collider[CollidersBufferSize];
    private readonly RaycastHit[] _raycastHitsBuffer = new RaycastHit[CollidersBufferSize];

    public bool IsGrounded()
    {
        switch (_overlapGameObject)
        {
            case OverlapGameObject.Box:
                return Physics.OverlapBoxNonAlloc(transform.position + _boxCenterOffset, _boxSize / 2, _collidersBuffer, Quaternion.identity, _layerMask) > 0;
            case OverlapGameObject.Capsule:
                return Physics.OverlapCapsuleNonAlloc(_capsuleFirstPoint, _capsuleSecondPoint, _capsuleRadius, _collidersBuffer, _layerMask) > 0;
            case OverlapGameObject.Sphere:
                return Physics.OverlapSphereNonAlloc(transform.position + _sphereCenterOffset, _sphereRadius, _collidersBuffer, _layerMask) > 0;
        }
        return false;
    }

    public bool IsGroundedByRaycast()
    {
        return Physics.RaycastNonAlloc(transform.position, Vector3.down, _raycastHitsBuffer, _rayDistance, _layerMask) > 0;
    }

    private void OnDrawGizmos()
    {
        switch (_overlapGameObject)
        {
            case OverlapGameObject.Box:
                Gizmos.DrawCube(transform.position + _boxCenterOffset, _boxSize);
                break;
/*            case OverlapGameObject.Capsule:
                GizmosExtensions.DrawCapsule(_gameObject.transform.position, _capsuleRadius, _capsuleHeight);
                break;*/
            case OverlapGameObject.Sphere:
                Gizmos.DrawSphere(transform.position + _sphereCenterOffset, _sphereRadius);
                break;
        }
        Gizmos.DrawRay(transform.position, Vector3.down * _rayDistance);
    }
}