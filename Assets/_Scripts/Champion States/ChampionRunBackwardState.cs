using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChampionRunBackwardState : ChampionStaminaState
{
    private readonly MoveController _moveController;
    private readonly float _maxTimeToMoveCancel = 0.1f;
    private float _currentTime = 0.0f;

    public ChampionRunBackwardState(Animator animator, IChampionStateSwitcher championStateSwitcher,
        GameControls.MoveListActions moveListActions, MoveController moveController,Champion champion) 
        : base(animator, championStateSwitcher, moveListActions, champion, 25.0f)
    {
        _moveController = moveController;
    }

    public override void Enter()
    {
        base.Enter();
        _currentTime = 0.0f;
        Animator.SetInteger(Constants.Animator.Params.CaracterMoveDirection, Constants.Direction.Left);
        Animator.SetBool(Constants.Animator.Params.IsCharacterRunning, true);
        OnStaminaEnded += StaminaEnded;
        MoveListActions.Move.canceled += MoveCanceled;
        MoveListActions.Run.canceled += RunCanceled;
    }

    public override void FixedUpdate()
    {
        if (Champion.ViewDirection == ViewDirection.Right)
        {
            _moveController.Run(Vector3.left);
        }
        else if (Champion.ViewDirection == ViewDirection.Left)
        {
            _moveController.Run(Vector3.right);
        }
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        base.Exit();
        Animator.SetBool(Constants.Animator.Params.IsCharacterRunning, false);
        OnStaminaEnded -= StaminaEnded;
        MoveListActions.Move.canceled -= MoveCanceled;
        MoveListActions.Run.canceled -= RunCanceled;
    }

    private void RunCanceled(InputAction.CallbackContext obj)
    {
        StartTimeToTrySwitchStateToMove();
    }

    private void MoveCanceled(InputAction.CallbackContext obj)
    {
        Animator.SetInteger(Constants.Animator.Params.CaracterMoveDirection, Constants.Direction.Zero);
        ChampionStateSwitcher.SwitchStateInstantly<ChampionIdleState>();
    }

    private async void StartTimeToTrySwitchStateToMove()
    {
        while(MoveListActions.Move.phase == InputActionPhase.Performed)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime > _maxTimeToMoveCancel)
            {
                ChampionStateSwitcher.SwitchStateInstantly<ChampionMoveBackwardState>();
                break;
            }
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
        }
    }

    private void StaminaEnded()
    {
        ChampionStateSwitcher.SwitchStateInstantly<ChampionMoveBackwardState>();
    }
}