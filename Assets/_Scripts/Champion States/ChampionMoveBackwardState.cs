using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChampionMoveBackwardState : ChampionBaseState
{
    private readonly MoveController _moveController;
    private float _maxTimeToSwitchMoveDirection = 0.11f;
    private float _currentTime = 0.0f;

    public ChampionMoveBackwardState(Animator animator, MoveController moveController,
        IChampionStateSwitcher championStateSwitcher, GameControls.MoveListActions moveListActions, Champion champion) 
        : base(animator, championStateSwitcher, moveListActions, champion)
    {
        _moveController = moveController;
    }

    public override void Enter()
    {
        _currentTime = 0.0f;
        Animator.SetInteger(Constants.Animator.Params.CaracterMoveDirection, Constants.Direction.Left);
        MoveListActions.Move.canceled += MoveCanceled;
        MoveListActions.Run.started += RunStarted;
        MoveListActions.Crouch.started += CrouchStarted;
        MoveListActions.Crouch.canceled += CrouchCanceled;
    }

    public override void FixedUpdate()
    {
        if (Champion.ViewDirection == ViewDirection.Right)
        {
            _moveController.Move(Vector3.left);
        }
        else if (Champion.ViewDirection == ViewDirection.Left)
        {
            _moveController.Move(Vector3.right);
        }
    }

    public override void Update()
    {
        if (MoveListActions.Move.phase == InputActionPhase.Performed)
        {
            sbyte moveDirection = (sbyte)MoveListActions.Move.ReadValue<float>();
            if (Champion.ViewDirection == ViewDirection.Right)
            {
                if (moveDirection == 1)
                {
                    ChampionStateSwitcher.SwitchStateInstantly<ChampionMoveForwardState>();
                }
            }
            else if (Champion.ViewDirection == ViewDirection.Left)
            {
                if (moveDirection == -1)
                {
                    ChampionStateSwitcher.SwitchStateInstantly<ChampionMoveForwardState>();
                }
            }
        }
    }

    public override void Exit()
    {
        Animator.SetInteger(Constants.Animator.Params.CaracterMoveDirection, Constants.Direction.Zero);
        MoveListActions.Move.canceled -= MoveCanceled;
        MoveListActions.Run.started -= RunStarted;
        MoveListActions.Crouch.started -= CrouchStarted;
        MoveListActions.Crouch.canceled -= CrouchCanceled;
    }

    private void MoveCanceled(InputAction.CallbackContext obj)
    {
        StartTimeToTrySwitchMoveDirection();
    }

    private void RunStarted(InputAction.CallbackContext obj)
    {
        ChampionStateSwitcher.SwitchStateInstantly<ChampionRunBackwardState>();
    }

    private void CrouchCanceled(InputAction.CallbackContext obj)
    {
        Animator.SetBool(Constants.Animator.Params.IsCharacterCrouching, false);
    }

    private void CrouchStarted(InputAction.CallbackContext obj)
    {
        Animator.SetBool(Constants.Animator.Params.IsCharacterCrouching, true);
    }

    private async void StartTimeToTrySwitchMoveDirection()
    {
        while (MoveListActions.Move.phase == InputActionPhase.Canceled || MoveListActions.Move.phase == InputActionPhase.Waiting)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime > _maxTimeToSwitchMoveDirection)
            {
                ChampionStateSwitcher.SwitchStateInstantly<ChampionIdleState>();
                break;
            }
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
        }
        if (_currentTime < _maxTimeToSwitchMoveDirection)
        {
            ChampionStateSwitcher.SwitchStateInstantly<ChampionMoveForwardState>();
        }
    }
}