using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class ConfirmationWindow : BaseView
{
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private TMP_Text _descriptionText;

    private AudioClip _buttonClickedAudioClip;
    private TMP_Text ConfirmButtonText => _confirmButton.GetComponentInChildren<TMP_Text>();
    private TMP_Text CancelButtonText => _cancelButton.GetComponentInChildren<TMP_Text>();
    private Action _onConfirm;
    private Action _onCancel;

    public Button ConfirmButton => _confirmButton;
    public Button CancelButton => _cancelButton;

    public void Initialize(Action onConfirm, Action onCancel, LocalizedString description, LocalizedString confirmButtonText, LocalizedString cancelButtonText)
    {
        _onConfirm = onConfirm;
        _onCancel = onCancel;
        _descriptionText.text = description.GetLocalizedString();
        ConfirmButtonText.text = confirmButtonText.GetLocalizedString();
        CancelButtonText.text = cancelButtonText.GetLocalizedString();
        _buttonClickedAudioClip = GameContext.Instance.AudioClipFactory.GetAudioClip(AudioClipType.ButtonClicked);
        _confirmButton.onClick.AddListener(ConfirmClicked);
        _cancelButton.onClick.AddListener(CancelClicked);
        _confirmButton.Select();
    }

    public override void Hide()
    {
        CancelClicked();
    }

    public virtual void DisableConfirmButton()
    {
        _confirmButton.navigation = new Navigation();
        _cancelButton.SetSelectableOnLeft(null);
        _confirmButton.interactable = false;
    }

    public virtual void EnableConfirmButton()
    {
        _confirmButton.SetSelectableOnRight(_cancelButton);
        _cancelButton.SetSelectableOnLeft(_confirmButton);
        _confirmButton.interactable = true;
    }

    public void DisableCancelButton()
    {
        _cancelButton.navigation = new Navigation();
        _cancelButton.interactable = false;
    }

    private void ConfirmClicked()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        //base.Hide();
        _onConfirm?.Invoke();
    }

    private void CancelClicked()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        base.Hide();
        _onCancel?.Invoke();
    }

    private void OnDestroy()
    {
        _confirmButton.onClick.RemoveListener(ConfirmClicked);
        _cancelButton.onClick.RemoveListener(CancelClicked);
    }
}