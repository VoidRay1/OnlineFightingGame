using System;
using UnityEngine;
using UnityEngine.UI;

public class GlobalMainMenuView : BaseView
{
    [SerializeField] private MainMenuView _mainMenuView;
    [SerializeField] private PlayerProfileView _playerProfileView;

    private MemberOfGroupProfileView _memberOfGroupProfileView;

    public MainMenuView MainMenuView => _mainMenuView;
    public PlayerProfileView PlayerProfileView => _playerProfileView;

    public event Action OnShowSettingsMenu;
    public event Action OnShowPlayerDetailProfile;

    public void Initialize(Selectable firstSelectedTab, PlayerData playerData, GameCloser gameCloser)
    {
        _mainMenuView.Initialize(firstSelectedTab, gameCloser);
        _playerProfileView.Initialize(playerData);
    }

    public void SetMemberOfGroupProfileView(MemberOfGroupProfileView memberOfGroupProfileView)
    {
        _memberOfGroupProfileView = memberOfGroupProfileView;
    }

    public override void Show()
    {
        SubscribeOnEvents();
        EnableGraphicRaycasters();
        _mainMenuView.HighlightSelectedTab();
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
        UnSubscribeOnEvents();
        DisableGraphicRaycasters();
    }

    public void UpdateUIWhenFindingMatchForNoGroup()
    { 
        _mainMenuView.ShowCancelFindingMatchButton();
        _mainMenuView.DisableGameButtons();
        _playerProfileView.HideAllGroupButtons();
    }

    public void UpdateUIWhenNoFindingMatchForNoGroup()
    {
        _mainMenuView.HideCancelFindingMatchButton();
        _mainMenuView.EnableGameButtons();
        _playerProfileView.ShowButtonsForNoActiveGroup();
    }

    public void UpdateUIWhenFindingMatchWithGroup()
    {
        UpdateUIWhenFindingMatchForNoGroup();
        if (_memberOfGroupProfileView != null)
        { 
            _memberOfGroupProfileView.HideLeaderButtonsForMember();
        }
    }

    public void UpdateUIWhenNoFindingMatchWithGroup(bool isGroupLeader)
    {
        _mainMenuView.HideCancelFindingMatchButton();
        _mainMenuView.EnableGameButtons();
        if (_memberOfGroupProfileView != null)
        {
            if (isGroupLeader)
            {
                _playerProfileView.ShowButtonsForLeader();
                _memberOfGroupProfileView.ShowButtonsForLeader();
            }
            else
            {
                _playerProfileView.ShowButtonsForMember();
                _memberOfGroupProfileView.HideLeaderButtonsForMember();
            }
        }
    }

    public void EnableDisableButtonsWhenFindingMatch()
    {
        _mainMenuView.DisableGameButtons();
        _playerProfileView.HideAllGroupButtons();
        _playerProfileView.SetSelectableOnLeft(_mainMenuView.CancelFindingMatchButton);
    }

    public void EnableDisableButtonsWhenNoFindingMatch()
    {
        _mainMenuView.EnableGameButtons();
        _playerProfileView.ShowButtonsForNoActiveGroup();
        _playerProfileView.SetSelectableOnLeft(_mainMenuView.OneVsOneOnlineButton);
    }

    private void ShowSettingsMenu()
    {
        OnShowSettingsMenu?.Invoke();
    }

    private void ShowPlayerDetailProfile()
    {
        OnShowPlayerDetailProfile?.Invoke();
    }

    private void DisableGraphicRaycasters()
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out GraphicRaycaster graphicRaycaster))
            {
                graphicRaycaster.enabled = false;
            }
        }
    }

    private void EnableGraphicRaycasters()
    {
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out GraphicRaycaster graphicRaycaster))
            {
                graphicRaycaster.enabled = true;
            }
        }
    }

    private void SubscribeOnEvents()
    {
        _mainMenuView.OnShowSettingsMenu += ShowSettingsMenu;
        _playerProfileView.OnShowPlayerDetailProfile += ShowPlayerDetailProfile;
    }

    private void UnSubscribeOnEvents()
    {
        _mainMenuView.OnShowSettingsMenu -= ShowSettingsMenu;
        _playerProfileView.OnShowPlayerDetailProfile -= ShowPlayerDetailProfile;
    }

    private void OnDestroy()
    {
        UnSubscribeOnEvents();
    }
}