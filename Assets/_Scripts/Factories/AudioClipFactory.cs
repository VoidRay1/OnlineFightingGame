using UnityEngine;

[CreateAssetMenu(menuName = "Factories/Audio Clip Factory")]
public class AudioClipFactory : ScriptableObject
{
    [SerializeField] private AudioClip _buttonClickedSound;

    public AudioClip GetAudioClip(AudioClipType audioClipType)
    {
        switch (audioClipType)
        {
            case AudioClipType.ButtonClicked:
                return _buttonClickedSound;
        }
        return null;
    }
}