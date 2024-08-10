using System.Collections.Generic;
using System.Linq;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Localization;

public class GroupCreator
{
    private readonly GroupCodeViewProvider _groupCodeViewProvider = new GroupCodeViewProvider();
    private readonly MemberOfGroupProfileViewProvider _memberOfGroupProfileViewProvider = new MemberOfGroupProfileViewProvider();
    private readonly List<Player> _players = new List<Player>();
    private readonly Transform _globalMainMenuViewParent;
    private readonly PlayerProfileView _playerProfileView;
    private readonly GlobalMainMenuView _globalMainMenuView;
    private MemberOfGroupProfileView _memberOfGroupProfileView;

    private GroupCodeView _groupCodeView;
    private Lobby _activeGroup;
    private GroupState _groupState;

    private InputWindowProvider InputWindowProvider => GameContext.Instance.InputWindowProvider;
    private PopupMessageProvider PopupMessageProvider => GameContext.Instance.PopupMessageProvider;
    private PlayerData PlayerData => GameContext.Instance.PlayerData;

    private const string MainMenuTable = Constants.Localization.TableReferences.MainMenuTable;
    private const string PlayerNameKey = "Name";
    private const string PlayerActivityKey = "Activity";
    private const string Description = "Enter group code";
    private const string ConfirmText = "Join";

    public bool IsGroupActive => _activeGroup != null;
    public bool IsPlayerGroupLeader => _activeGroup != null && _activeGroup.HostId == PlayerData.Id;
    public IReadOnlyList<Player> Players => _players;

    public GroupCreator(GlobalMainMenuView globalMainMenuView)
    {
        _groupState = GroupState.NoActiveGroup;
        _globalMainMenuViewParent = globalMainMenuView.Canvas.transform;
        _playerProfileView = globalMainMenuView.PlayerProfileView;
        _globalMainMenuView = globalMainMenuView;
        _players.Add(GetPlayer());
        SubscribeOnEvents();
        DisplayGroupButtons();
    }

    private void ShowGroupCodeInputWindow()
    {
        InputWindowProvider.ShowInputWindow(Constants.Addressables.Keys.GroupCodeInputWindow,
            TryJoinGroup, CloseGroupCodeInputWindow,
            new LocalizedString
            {
                TableReference = MainMenuTable,
                TableEntryReference = Description
            },
            new LocalizedString
            {
                TableReference = MainMenuTable,
                TableEntryReference = ConfirmText
            },
            new LocalizedString
            {
                TableReference = MainMenuTable,
                TableEntryReference = Constants.Localization.TableEntryReferences.Cancel
            });
    }

    private void CloseGroupCodeInputWindow()
    {
        InputWindowProvider.UnloadInputWindow();
        _playerProfileView.SelectJoinGroupButton();
    }

    private async void TryJoinGroup(string groupCode)
    {
        try
        {
            _globalMainMenuView.EnableDisableButtonsWhenFindingMatch();
            InputWindowProvider.HideInputWindow();
            var lobbyOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer()
            };
            _activeGroup = await LobbyService.Instance.JoinLobbyByCodeAsync(groupCode, lobbyOptions);
            await LobbyService.Instance.SubscribeToLobbyEventsAsync(_activeGroup.Id, GetGroupEvents());
            var host = _activeGroup.Players.Where(player => player.Id == _activeGroup.HostId).First();
            _memberOfGroupProfileView = await _memberOfGroupProfileViewProvider.ShowMemberOfGroupProfileView(
                new PlayerData(host.Id, host.Data[PlayerNameKey].Value, host.Data[PlayerActivityKey].Value), _globalMainMenuViewParent);
            _globalMainMenuView.SetMemberOfGroupProfileView(_memberOfGroupProfileView);
            _players.Add(host);
            _memberOfGroupProfileView.ShowGroupLeaderImage();
            _groupState = GroupState.GroupIsFull;
            InputWindowProvider.UnloadInputWindow();
            DisplayGroupButtons();
        }
        catch (LobbyServiceException)
        {
            PopupMessageProvider.ShowErrorMessage("Error when trying to join a group");
            InputWindowProvider.ShowInputWindow();
        }
    }

    private async void MakeMemberLeaderOfGroup(PlayerData playerData)
    {
        try
        {
            _memberOfGroupProfileView.HideLeaderButtonsForMember();
            var lobbyOptions = new UpdateLobbyOptions()
            {
                HostId = playerData.Id
            };
            _activeGroup = await LobbyService.Instance.UpdateLobbyAsync(_activeGroup.Id, lobbyOptions);
            DisplayGroupButtons();
        }
        catch (LobbyServiceException)
        {
            PopupMessageProvider.ShowErrorMessage("Error when trying to make member leader of group");
        }
    }

    private void KickedFromGroup()
    {
        _memberOfGroupProfileViewProvider.UnloadMemberOfGroupProfileView();
        _globalMainMenuView.SetMemberOfGroupProfileView(null);
        _memberOfGroupProfileView = null;
        _activeGroup = null;
        _groupState = GroupState.NoActiveGroup;
        DisplayGroupButtons();
    }

    private async void KickMemberFromGroup(PlayerData playerData)
    {
        try
        {
            _memberOfGroupProfileView.HideLeaderButtonsForMember();
            await LobbyService.Instance.RemovePlayerAsync(_activeGroup.Id, playerData.Id);
            _memberOfGroupProfileViewProvider.UnloadMemberOfGroupProfileView();
            _globalMainMenuView.SetMemberOfGroupProfileView(null);
            _memberOfGroupProfileView = null;
            _groupState = GroupState.WaitingPlayersToJoin;
            DisplayGroupButtons();
        }
        catch (LobbyServiceException)
        {
            PopupMessageProvider.ShowErrorMessage("Error when trying to kick member of group");
        }
    }

    private async void TryCreateGroup()
    {
        try
        {
            _globalMainMenuView.EnableDisableButtonsWhenFindingMatch();
            var lobbyOptions = new CreateLobbyOptions
            {
                Player = GetPlayer(),
                IsPrivate = true,
            };
            string lobbyName = PlayerData.Id.ToString();
            _activeGroup = await LobbyService.Instance.CreateLobbyAsync(lobbyName, Constants.MaxPlayers, lobbyOptions);
            await LobbyService.Instance.SubscribeToLobbyEventsAsync(_activeGroup.Id, GetGroupEvents());
            _groupState = GroupState.WaitingPlayersToJoin;
            DisplayGroupButtons();
        }
        catch (LobbyServiceException)
        {
            PopupMessageProvider.ShowErrorMessage("Error when trying to create group");
        }
    }

    private void GroupDeleted()
    {
        _memberOfGroupProfileViewProvider.UnloadMemberOfGroupProfileView();
        _groupState = GroupState.NoActiveGroup;
        DisplayGroupButtons();
    }

    private void GroupChanged(ILobbyChanges lobbyChanges)
    {
        if (lobbyChanges.LobbyDeleted == false)
        {
            lobbyChanges.ApplyToLobby(_activeGroup);
        }
        DisplayGroupButtons();
    }

    private void PlayerLeft(List<int> playerIndex)
    {
        _memberOfGroupProfileViewProvider.UnloadMemberOfGroupProfileView();
        _memberOfGroupProfileView = null;
        _globalMainMenuView.SetMemberOfGroupProfileView(null);
        _groupState = GroupState.WaitingPlayersToJoin;
        var playerLeft = _players.Where(player => player.Id != PlayerData.Id).First();
        _players.Remove(playerLeft);
        DisplayGroupButtons();
    }

    private async void PlayerJoined(List<LobbyPlayerJoined> playerJoined)
    {
        Player joinedPlayer = playerJoined[0].Player;
        _memberOfGroupProfileView = await _memberOfGroupProfileViewProvider.ShowMemberOfGroupProfileView(
            new PlayerData(joinedPlayer.Id, joinedPlayer.Data[PlayerNameKey].Value, joinedPlayer.Data[PlayerActivityKey].Value), _globalMainMenuViewParent);
        _globalMainMenuView.SetMemberOfGroupProfileView(_memberOfGroupProfileView);
        _memberOfGroupProfileView.OnKickMemberFromGroup += KickMemberFromGroup;
        _memberOfGroupProfileView.OnMakeMemberLeaderOfGroup += MakeMemberLeaderOfGroup;
        _groupState = GroupState.GroupIsFull;
        _players.Add(joinedPlayer);
        DisplayGroupButtons();
    }

    private async void CloseGroup()
    {
        try
        {
            _playerProfileView.HideCloseGroupButton();
            await LobbyService.Instance.DeleteLobbyAsync(_activeGroup.Id);
            _groupCodeViewProvider.UnloadGroupCodeView();
            _memberOfGroupProfileViewProvider.UnloadMemberOfGroupProfileView();
            _globalMainMenuView.SetMemberOfGroupProfileView(null);
            _activeGroup = null;
            _groupState = GroupState.NoActiveGroup;
            DisplayGroupButtons();
        }
        catch (LobbyServiceException)
        {
            PopupMessageProvider.ShowErrorMessage("Error when trying to close group");
        }
    }

    public void TryLeaveGroup(PlayerData playerData)
    {
        if (_activeGroup == null)
        {
            return;
        }
        LeaveGroup(playerData);
    }

    private async void LeaveGroup(PlayerData playerData)
    {
        try
        {
            _playerProfileView.HideLeaveGroupButton();
            await LobbyService.Instance.RemovePlayerAsync(_activeGroup.Id, playerData.Id);
            _memberOfGroupProfileViewProvider.UnloadMemberOfGroupProfileView();
            _memberOfGroupProfileView = null;
            _globalMainMenuView.SetMemberOfGroupProfileView(null);
            _activeGroup = null;
            _groupState = GroupState.NoActiveGroup;
            DisplayGroupButtons();
        }
        catch (LobbyServiceException)
        {
            PopupMessageProvider.ShowErrorMessage("Error when trying to leave group");
        }
    }

    private Player GetPlayer()
    {
        return new Player(
            id: PlayerData.Id,
            data: new Dictionary<string, PlayerDataObject>
            {
                [PlayerNameKey] = new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, PlayerData.Name),
                [PlayerActivityKey] = new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, PlayerData.Activity),
            });
    }

    public async void DisplayGroupButtons()
    {
        switch (_groupState)
        {
            case GroupState.NoActiveGroup:
                _globalMainMenuView.EnableDisableButtonsWhenNoFindingMatch();
                _playerProfileView.ShowButtonsForNoActiveGroup();
                break;
            case GroupState.WaitingPlayersToJoin:
                if (_groupCodeView == null)
                {
                    _groupCodeView = await _groupCodeViewProvider.ShowGroupCodeView(_activeGroup.LobbyCode, _globalMainMenuViewParent);
                }
                else
                {
                    _groupCodeView.Show();
                }
                _globalMainMenuView.EnableDisableButtonsWhenFindingMatch();
                _playerProfileView.ShowButtonsForNotFullGroup();
                break;
            case GroupState.GroupIsFull:
                if (_activeGroup.HostId == PlayerData.Id)
                {
                    if (_groupCodeView != null)
                    {
                        _groupCodeView.Hide();
                    }
                    _globalMainMenuView.EnableDisableButtonsWhenNoFindingMatch();
                    _playerProfileView.ShowGroupLeaderImage();
                    _playerProfileView.ShowButtonsForLeader();
                    _memberOfGroupProfileView.HideGroupLeaderImage();
                    _memberOfGroupProfileView.ShowButtonsForLeader();
                }
                else
                {
                    _globalMainMenuView.EnableDisableButtonsWhenFindingMatch();
                    _playerProfileView.HideGroupLeaderImage();
                    _playerProfileView.ShowButtonsForMember();
                    _memberOfGroupProfileView.ShowGroupLeaderImage();
                    _memberOfGroupProfileView.HideLeaderButtonsForMember();
                }
                break;
        }
    }

    private void PlayerProfileDestroyed()
    {
        UnSubscribeOnEvents();
    }

    private LobbyEventCallbacks GetGroupEvents()
    {
        var lobbyEvents = new LobbyEventCallbacks();
        lobbyEvents.LobbyChanged += GroupChanged;
        lobbyEvents.LobbyDeleted += GroupDeleted;
        lobbyEvents.PlayerJoined += PlayerJoined;
        lobbyEvents.PlayerLeft += PlayerLeft;
        lobbyEvents.KickedFromLobby += KickedFromGroup;
        return lobbyEvents;
    }

    private void SubscribeOnEvents()
    {
        _playerProfileView.OnJoinGroup += ShowGroupCodeInputWindow;
        _playerProfileView.OnCreateGroup += TryCreateGroup;
        _playerProfileView.OnCloseGroup += CloseGroup;
        _playerProfileView.OnLeaveGroup += TryLeaveGroup;
        _playerProfileView.OnPlayerProfileDestroyed += PlayerProfileDestroyed;
    }

    private void UnSubscribeOnEvents()
    {
        _playerProfileView.OnJoinGroup -= ShowGroupCodeInputWindow;
        _playerProfileView.OnCreateGroup -= TryCreateGroup;
        _playerProfileView.OnCloseGroup -= CloseGroup;
        _playerProfileView.OnLeaveGroup -= TryLeaveGroup;
        _playerProfileView.OnPlayerProfileDestroyed -= PlayerProfileDestroyed;
    }
}