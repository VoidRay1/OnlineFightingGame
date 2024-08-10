using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Multiplay;
using UnityEngine;

public class ServerMultiplayService : MonoBehaviour
{
#if SERVER
    private static ServerMultiplayService _instance;

    private ServerConfig ServerConfig => MultiplayService.Instance.ServerConfig;

    private IServerQueryHandler _serverQueryHandler;
    private bool _isAllocated;
    private const string Address0000 = "0.0.0.0";

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async void Start()
    {
        SetTargetFramerate();
        await SubscribeOnMultiplayEvents();
        await TryStartServer();
    }

    private void Update()
    {
        if (_serverQueryHandler != null)
        {
            _serverQueryHandler.UpdateServerCheck();
        }
    }

    private async Task TryStartServer()
    {
        try
        {
            _serverQueryHandler = await MultiplayService.Instance.StartServerQueryHandlerAsync(Constants.MaxPlayers, "n/a", "n/a", "0", "n/a");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void ApproveConnection(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if (NetworkManager.Singleton.ConnectedClients.Count > Constants.MaxPlayers)
        {
            response.Approved = false;
            response.Reason = "Server is full";
        }
        else
        {
            response.Approved = true;
        }
    }

    private async Task SubscribeOnMultiplayEvents()
    {
        MultiplayEventCallbacks multiplayEvents = new MultiplayEventCallbacks();
        multiplayEvents.Allocate += ServerAllocated;
        multiplayEvents.Deallocate += ServerDeallocated;
        multiplayEvents.Error += ServerErrorReceived;
        await MultiplayService.Instance.SubscribeToServerEventsAsync(multiplayEvents);
    }

    private void ServerAllocated(MultiplayAllocation multiplayAllocation)
    {
        print("Server allocated");
        if (_isAllocated)
        {
            return;
        }
        _isAllocated = true;
        print($"Server Id: {ServerConfig.ServerId}\n" +
            $"Allocation Id: {ServerConfig.AllocationId}\n" +
            $"Port: {ServerConfig.Port}\n" +
            $"Query Port: {ServerConfig.QueryPort}\n" +
            $"Ip: {ServerConfig.IpAddress}\n" +
            $"Logs: {ServerConfig.ServerLogDirectory}\n");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(Address0000, ServerConfig.Port, Address0000);
        NetworkManager.Singleton.ConnectionApprovalCallback += ApproveConnection;
        NetworkManager.Singleton.OnClientConnectedCallback += ClientConnected;
        NetworkManager.Singleton.StartServer();
    }

    private void ClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.ConnectedClients.Count == Constants.MaxPlayers)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= ClientConnected;
            NetworkManager.Singleton.SceneManager.LoadScene(Constants.Scenes.FightArena, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }

    private void ServerDeallocated(MultiplayDeallocation multiplayDeallocation)
    {
        
    }

    private void ServerErrorReceived(MultiplayError multiplayError)
    {
        print($"Error Details: {multiplayError.Detail}\n" +
            $"Error Reason: {multiplayError.Reason}\n");
    }

    private void SetTargetFramerate()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
#endif
}