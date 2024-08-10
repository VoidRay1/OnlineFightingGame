using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Matchmaker;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

public class Matchmaker : MonoBehaviour
{
    [SerializeField] private GlobalMainMenuView _globalMainMenu;

    private readonly FindingMatchViewProvider _findingMatchViewProvider = new FindingMatchViewProvider();
    private PopupMessageProvider PopupMessageProvider => GameContext.Instance.PopupMessageProvider;

    private Transform _globalMainMenuViewParent;
    private CreateTicketResponse _createdTicketResponse;
    private GroupCreator _groupCreator;
    private FindingMatchView _findingMatchView;
    private IEnumerator _findingMatchTimerCoroutine;
    private int _elapsedFindingMatchTime;
    private float _ticketCooldown;
    private const byte DecreaseTime = 1;
    private const float TicketMaxCooldown = 1.2f;
    private const string QueueName = "Default";

    public void Initialize(GroupCreator groupCreator)
    {
        _groupCreator = groupCreator;
        _globalMainMenuViewParent = _globalMainMenu.Canvas.transform;
        _globalMainMenu.MainMenuView.HideCancelFindingMatchButton();
        _globalMainMenu.MainMenuView.OnCancelFindingMatch += CancelFindingMatch;
        _globalMainMenu.MainMenuView.OnOneVsOneOnlineClicked += OneVsOne;
    }

    private void Update()
    {
        if (_createdTicketResponse != null)
        {
            _ticketCooldown -= Time.deltaTime;
            if ( _ticketCooldown < 0) 
            {
                _ticketCooldown = TicketMaxCooldown;
                GetPollTicketStatus();
            }
        }
    }

    private async void CancelFindingMatch()
    {
        if (_groupCreator.IsGroupActive)
        {
            _globalMainMenu.UpdateUIWhenNoFindingMatchWithGroup(_groupCreator.IsPlayerGroupLeader);
        }
        else
        {
            _globalMainMenu.UpdateUIWhenNoFindingMatchForNoGroup();
        }
        try
        {
            await MatchmakerService.Instance.DeleteTicketAsync(_createdTicketResponse.Id);
        }
        catch (MatchmakerServiceException)
        {
            PopupMessageProvider.ShowErrorMessage("Failure to cancel match");
        }
        finally
        {
            Reset();
        }
    }

    private async void OneVsOne()
    {
        if (_groupCreator.IsGroupActive)
        {
            _globalMainMenu.UpdateUIWhenFindingMatchWithGroup();
        }
        else
        {
            _globalMainMenu.UpdateUIWhenFindingMatchForNoGroup();
        }
        List<Player> players = new List<Player>();
        foreach (var lobbyPlayer in _groupCreator.Players) 
        {
            var matchmakerPlayer = new Player(lobbyPlayer.Id);
            players.Add(matchmakerPlayer);
        }
        var ticketOptions = new CreateTicketOptions(QueueName);
        _createdTicketResponse = await MatchmakerService.Instance.CreateTicketAsync(players, ticketOptions);
        _globalMainMenu.MainMenuView.ShowCancelFindingMatchButton();
        _findingMatchView = await _findingMatchViewProvider.LoadFindingMatchView(_globalMainMenuViewParent);
        _findingMatchTimerCoroutine = StartFindingMatchTimer();
        StartCoroutine(_findingMatchTimerCoroutine);
        _ticketCooldown = TicketMaxCooldown;
    }

    private async void GetPollTicketStatus()
    {
        var ticketStatusResponse = await MatchmakerService.Instance.GetTicketAsync(_createdTicketResponse.Id);
        if (ticketStatusResponse == null) 
        {
            return;
        }
        if (ticketStatusResponse.Type == typeof(MultiplayAssignment)) 
        {
            var multiplayAssignment = ticketStatusResponse.Value as MultiplayAssignment;
            switch (multiplayAssignment.Status)
            {
                case MultiplayAssignment.StatusOptions.Found:
                    Reset();
                    NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(multiplayAssignment.Ip, (ushort)multiplayAssignment.Port);
                    NetworkManager.Singleton.StartClient();
                    Debug.Log("Match founded");
                    break;
                case MultiplayAssignment.StatusOptions.InProgress:
                    Debug.Log("Looking for match");
                    break;
                case MultiplayAssignment.StatusOptions.Timeout:
                    Reset();
                    _globalMainMenu.EnableDisableButtonsWhenNoFindingMatch();
                    PopupMessageProvider.ShowNeutralMessage("Match not found, try again");
                    break;
                case MultiplayAssignment.StatusOptions.Failed:
                    Debug.Log(multiplayAssignment.Message);
                    Reset();
                    _globalMainMenu.EnableDisableButtonsWhenNoFindingMatch();
                    PopupMessageProvider.ShowErrorMessage("Failed to connect to match, try again");
                    break;
            }
        }
    }

    private void Reset()
    {
        _createdTicketResponse = null;
        StopCoroutine(_findingMatchTimerCoroutine);
        _globalMainMenu.MainMenuView.HideCancelFindingMatchButton();
        _findingMatchViewProvider.UnloadFindingMatchView();
        _elapsedFindingMatchTime = 0;
    }

    private IEnumerator StartFindingMatchTimer()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(DecreaseTime);
        while (_createdTicketResponse != null)
        {
            _elapsedFindingMatchTime += DecreaseTime;
            _findingMatchView.DisplayTime(_elapsedFindingMatchTime);
            yield return waitForSeconds;
        }
    }

    private void OnDestroy()
    {
        _globalMainMenu.MainMenuView.OnCancelFindingMatch -= CancelFindingMatch;
        _globalMainMenu.MainMenuView.OnOneVsOneOnlineClicked -= OneVsOne;
    }
}