using UnityEngine;
using UnityEngine.InputSystem;

public class ChampionIdleState : ChampionBaseState
{
    private float _movePerformedTime;
    private const float MaxTimeToPerformMove = 0.1f;
    private readonly Champion _champion;

    public ChampionIdleState(Animator animator, IChampionStateSwitcher championStateSwitcher, GameControls.MoveListActions moveListActions, Champion champion)
        : base(animator, championStateSwitcher, moveListActions, champion)
    {
        _champion = champion;
    }

    public override void Enter()
    {
        if (_champion.IsActive)
        {
            _movePerformedTime = 0.0f;
            MoveListActions.Block.performed += BlockPerformed;
            MoveListActions.Punch.started += PunchStarted;
            MoveListActions.Uppercut.started += UppercutStarted;
            MoveListActions.LegKick.started += LegKickStarted;
            MoveListActions.Move.canceled += MoveCanceled;
            MoveListActions.StepBackward.performed += StepBackwardPerformed;
            MoveListActions.StepForward.performed += StepForwardPerformed;
        }
    }

    public override void Update()
    {
        if (MoveListActions.Jump.phase == InputActionPhase.Performed)
        {
            ChampionStateSwitcher.SwitchStateInstantly<ChampionInJumpState>();
        }
        if (MoveListActions.Crouch.phase == InputActionPhase.Performed)
        {
            ChampionStateSwitcher.SwitchStateInstantly<ChampionInCrouchState>();
        }   
        if (MoveListActions.Run.phase == InputActionPhase.Performed && MoveListActions.Move.phase == InputActionPhase.Performed)
        {
            sbyte moveDirection = (sbyte)MoveListActions.Move.ReadValue<float>();
            if (_champion.ViewDirection == ViewDirection.Right)
            {
                if (moveDirection == -1)
                {
                    ChampionStateSwitcher.SwitchStateInstantly<ChampionRunBackwardState>();
                }
                else if (moveDirection == 1)
                {
                    ChampionStateSwitcher.SwitchStateInstantly<ChampionRunForwardState>();
                }
            }
            else if (_champion.ViewDirection == ViewDirection.Left)
            {
                if (moveDirection == -1)
                {
                    ChampionStateSwitcher.SwitchStateInstantly<ChampionRunForwardState>();
                }
                else if (moveDirection == 1)
                {
                    ChampionStateSwitcher.SwitchStateInstantly<ChampionRunBackwardState>();
                }
            }
        }
        if (MoveListActions.Move.phase == InputActionPhase.Performed)
        {
            _movePerformedTime += Time.deltaTime;
            if (_movePerformedTime > MaxTimeToPerformMove)
            {
                sbyte moveDirection = (sbyte)MoveListActions.Move.ReadValue<float>();
                if (_champion.ViewDirection == ViewDirection.Right)
                {
                    if (moveDirection == -1)
                    {
                        ChampionStateSwitcher.SwitchStateInstantly<ChampionMoveBackwardState>();
                    }
                    else if (moveDirection == 1)
                    {
                        ChampionStateSwitcher.SwitchStateInstantly<ChampionMoveForwardState>();
                    }
                }
                else if(_champion.ViewDirection == ViewDirection.Left)
                {
                    if (moveDirection == -1)
                    {
                        ChampionStateSwitcher.SwitchStateInstantly<ChampionMoveForwardState>();
                    }
                    else if (moveDirection == 1)
                    {
                        ChampionStateSwitcher.SwitchStateInstantly<ChampionMoveBackwardState>();
                    }
                }
            }
        }
    }

    public override void Exit()
    {
        if (_champion.IsActive)
        {
            MoveListActions.Block.performed -= BlockPerformed;
            MoveListActions.Punch.started -= PunchStarted;
            MoveListActions.Uppercut.started -= UppercutStarted;
            MoveListActions.LegKick.started -= LegKickStarted;
            MoveListActions.Move.canceled -= MoveCanceled;
            MoveListActions.StepBackward.performed -= StepBackwardPerformed;
            MoveListActions.StepForward.performed -= StepForwardPerformed;
        }
    }

    private void MoveCanceled(InputAction.CallbackContext obj)
    {
        _movePerformedTime = 0.0f;
    }

    private void StepForwardPerformed(InputAction.CallbackContext obj)
    {
        if (Champion.ViewDirection == ViewDirection.Right)
        {
            ChampionStateSwitcher.SwitchStateInstantly<ChampionForwardStepState>();
        }
        else if (Champion.ViewDirection == ViewDirection.Left)
        {
            ChampionStateSwitcher.SwitchStateInstantly<ChampionBackwardStepState>();
        }
    }

    private void StepBackwardPerformed(InputAction.CallbackContext obj)
    {
        if (Champion.ViewDirection == ViewDirection.Right)
        {
            ChampionStateSwitcher.SwitchStateInstantly<ChampionBackwardStepState>();
        }
        else if(Champion.ViewDirection == ViewDirection.Left)
        {
            ChampionStateSwitcher.SwitchStateInstantly<ChampionForwardStepState>();
        }
    }

    private void BlockPerformed(InputAction.CallbackContext obj)
    {
        ChampionStateSwitcher.SwitchStateInstantly<ChampionInBlockState>();
    }

    private void PunchStarted(InputAction.CallbackContext obj)
    {
        ChampionStateSwitcher.SwitchStateInstantly<ChampionPunchState>();
    }

    private void UppercutStarted(InputAction.CallbackContext obj)
    {
        ChampionStateSwitcher.SwitchStateInstantly<ChampionUppercutFromIdleState>();
    }

    private void LegKickStarted(InputAction.CallbackContext obj)
    {
        ChampionStateSwitcher.SwitchStateInstantly<ChampionLegKickState>();
    }
}