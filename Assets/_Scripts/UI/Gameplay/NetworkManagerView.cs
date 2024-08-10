using System;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerView : BaseView
{
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _clientButton;

    public Action OnHostButtonClicked;
    public Action OnClientButtonClicked;

    public override void Show()
    {
        _hostButton.onClick.AddListener(HostButtonClicked);
        _clientButton.onClick.AddListener(ClientButtonClicked);
        base.Show();
    }

    private void HostButtonClicked()
    {
        OnHostButtonClicked?.Invoke();
        base.Hide();
    }

    private void ClientButtonClicked()
    {
        OnClientButtonClicked?.Invoke();
        base.Hide();
    }

    private void OnDestroy()
    {
        _hostButton.onClick.RemoveListener(HostButtonClicked);
        _clientButton.onClick.RemoveListener(ClientButtonClicked);
    }
}