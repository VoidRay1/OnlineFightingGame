using UnityEngine;

[CreateAssetMenu(menuName = "Attack Move/Hit Box Data")]
public class HitBoxData : ScriptableObject
{
    [SerializeField] private Vector3 _position;
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private Vector3 _size;

    public Vector3 Position => _position;
    public Quaternion Rotation => Quaternion.Euler(_rotation);
    public Vector3 ColliderSize => _size;
}