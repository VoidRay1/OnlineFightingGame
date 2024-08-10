using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class BasePlayerProfileView : BaseView
{
    [SerializeField] private RawImage _avatar;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _activity;
    [SerializeField] private Image _groupLeaderImage;
    [SerializeField] private Button _copyNameToClipBoardButton;

    private PlayerData _playerData;
    private AudioClip _buttonClickedAudioClip;

    public PlayerData PlayerData => _playerData;
    public AudioClip ButtonClickedAudioClip => _buttonClickedAudioClip;

    public virtual void Initialize(PlayerData playerData)
    {
        _playerData = playerData;
        _name.text = playerData.Name;
        _activity.text = playerData.Activity;
        SetAvatar(playerData.Avatar);
        _buttonClickedAudioClip = GameContext.Instance.AudioClipFactory.GetAudioClip(AudioClipType.ButtonClicked);
        _copyNameToClipBoardButton.onClick.AddListener(CopyNameToClipBoard);
    }

    public void SetAvatar(Texture2D texture)
    {
        _avatar.texture = texture != null ? texture : new Texture2D(0, 0);
    }

    public void ShowGroupLeaderImage()
    {
        _groupLeaderImage.enabled = true;
    }

    public void HideGroupLeaderImage()
    {
        _groupLeaderImage.enabled = false;
    }

    private void CopyNameToClipBoard()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        GUIUtility.systemCopyBuffer = _name.text;
    }

    private void OnDestroy()
    {
        _copyNameToClipBoardButton.onClick.RemoveListener(CopyNameToClipBoard);
    }
}