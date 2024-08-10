using UnityEngine;

public class AudioSourcePlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    public void PlayClip(AudioClip audioClip, float volume = 1.0f)
    {
        _audioSource.clip = audioClip;
        _audioSource.volume = volume;
        _audioSource.Play();
    }
}