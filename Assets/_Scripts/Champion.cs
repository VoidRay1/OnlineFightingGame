using Unity.Netcode;
using UnityEngine;

public class Champion : NetworkBehaviour
{
    [SerializeField] private HitBox _hitBox;
    [SerializeField] private HurtBox _hurtBox;
    [SerializeField] private ChampionStateMachine _championStateMachine;
    [SerializeField] private ChampionTracker _championTracker;
    private Health _health;
    private Stamina _stamina;
    private ChampionHud _hud;
    private ViewDirection _viewDirection;
    private bool _isActive;

    public bool IsActive => _isActive;
    public HitBox HitBox => _hitBox;
    public ChampionStateMachine StateMachine => _championStateMachine;
    public ChampionTracker Tracker => _championTracker;
    public Health Health => _health;
    public Stamina Stamina => _stamina;
    public Transform ChampionHead => _championTracker.ChampionHead;
    public ChampionHud Hud => _hud;
    public ViewDirection ViewDirection => _viewDirection;

    public void Initialize(Transform enemyChampionHead, ChampionHud championHud, GameControls.MoveListActions moveListActions, bool isActive)
    {
        _health = new Health(championHud.ChampionHealthDisplayer);
        _stamina = new Stamina(championHud.ChampionStaminaDisplayer);
        _hurtBox.Initialize(_health);
        _hud = championHud;
        _isActive = isActive;
        _championStateMachine.Initialize(moveListActions, this);
        _championTracker.Initialize(enemyChampionHead);
    }

    public void SetViewDirection(ViewDirection viewDirection)
    {
        _viewDirection = viewDirection;
    }

    public void ResetStats()
    {
        _health.ResetHealthToMax();
        _stamina.ResetStaminaToMax();
        _championTracker.Enable();
    }

    public void EnableHitBox()
    {
        _hitBox.Enable();
    }

    public void DisableHitBox()
    {
        _hitBox.Disable();
    }

    private void Update()
    {
        if(_stamina != null)
        {
            _stamina.TryRestore();
        }
    }

    private void LateUpdate()
    {
        _championTracker.SelfLateUpdate();
    }
}