using System.Collections.Generic;
using UnityEngine;

public class GameContext : MonoBehaviour
{
    [SerializeField] private AudioClipFactory _audioClipFactory;
    [SerializeField] private AudioSourcePlayer _audioSourcePlayer;
    private GameControls _gameControls;
    private WindowCloser _windowCloser;
    private SettingsData _settingsData;
    private AvatarsDataList _avatarsDataList;
    private PlayerData _playerData;
    private PopupMessageProvider _popupMessageProvider;
    private ConfirmationWindowProvider _confirmationWindowProvider;
    private InputWindowProvider _inputWindowProvider;
    private SettingsApplier _settingsApplier;
    private ISaveService _saveService;

    public List<Resolution> Resolutions = new List<Resolution>();

    public AudioClipFactory AudioClipFactory => _audioClipFactory;
    public AudioSourcePlayer AudioSourcePlayer => _audioSourcePlayer;
    public GameControls GameControls => _gameControls;
    public WindowCloser WindowCloser => _windowCloser;
    public SettingsData SettingsData => _settingsData;
    public AvatarsDataList AvatarsDataList => _avatarsDataList; 
    public PlayerData PlayerData => _playerData;
    public PopupMessageProvider PopupMessageProvider => _popupMessageProvider;
    public InputWindowProvider InputWindowProvider => _inputWindowProvider;
    public ConfirmationWindowProvider ConfirmationWindowProvider => _confirmationWindowProvider;
    public SettingsApplier SettingsApplier => _settingsApplier;
    public ISaveService SaveService => _saveService;
    public static GameContext Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void InitializeGameControls()
    {
        _gameControls = new GameControls();
        _gameControls.Window.Enable();
    }

    public void Initialize()
    {
        _saveService = new JsonSaveService();
        InitializeGameControls();
        _windowCloser = new WindowCloser(_gameControls.Window);
        _popupMessageProvider = new PopupMessageProvider();
        _inputWindowProvider = new InputWindowProvider();
        _confirmationWindowProvider = new ConfirmationWindowProvider();
        _settingsApplier = new SettingsApplier(_saveService);
    }

    public void SetSettingsData(SettingsData settingsData)
    {
        _settingsData = settingsData;
    }

    public void SetPlayerData(AvatarsDataList avatarsDataList, PlayerData playerData)
    {
        _avatarsDataList = avatarsDataList;
        _playerData = playerData;
    }
}