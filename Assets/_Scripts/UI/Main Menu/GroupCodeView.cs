using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

public class GroupCodeView : BaseView
{
    [SerializeField] private TMP_Text _groupCodeText;
    [SerializeField] private LocalizedString _localizedGroupCodeText;
    [SerializeField] private Button _copyGroupCodeToClipBoardButton;

    private AudioClip _buttonClickedAudioClip;
    private string _groupCode;

    private const string GroupCode = "GroupCode";
    
    public void Initialize()
    {
        _buttonClickedAudioClip = GameContext.Instance.AudioClipFactory.GetAudioClip(AudioClipType.ButtonClicked);
        _localizedGroupCodeText.Add(GroupCode, new StringVariable());
        _localizedGroupCodeText.StringChanged += GroupCodeChanged;
        _copyGroupCodeToClipBoardButton.onClick.AddListener(CopyGroupCodeToClipBoard);
    }

    public void ShowGroupCode(string groupCode)
    {
        _groupCode = groupCode;
        (_localizedGroupCodeText[GroupCode] as StringVariable).Value = groupCode;
        base.Show();
    }

    private void CopyGroupCodeToClipBoard()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        GUIUtility.systemCopyBuffer = _groupCode;
    }

    private void GroupCodeChanged(string value)
    {
        _groupCodeText.text = value;
    }

    private void OnDestroy()
    {
        _copyGroupCodeToClipBoardButton.onClick.RemoveListener(CopyGroupCodeToClipBoard);
        _localizedGroupCodeText.StringChanged -= GroupCodeChanged;
    }
}