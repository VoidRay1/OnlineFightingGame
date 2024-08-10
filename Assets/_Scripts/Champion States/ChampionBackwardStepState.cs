using Cysharp.Threading.Tasks;
using UnityEngine;

public class ChampionBackwardStepState : ChampionBaseState
{
    private readonly MoveController _moveController;
    private bool _isStarted = false;

    public ChampionBackwardStepState(Animator animator, MoveController moveController, IChampionStateSwitcher championStateSwitcher,
        GameControls.MoveListActions moveListActions, Champion champion)
        : base(animator, championStateSwitcher, moveListActions, champion)
    {
        _moveController = moveController;
    }

    public override void Enter()
    {
        Animator.SetTrigger(Constants.Animator.Params.StepBackwardTrigger);
        WaitForFrameEnd();
    }

    public override void FixedUpdate()
    {
        if (_isStarted == false)
        {
            return;
        }
        if (_moveController.IsStepCompleted == false)
        {
            _moveController.Step();
        }
        else
        {
            ChampionStateSwitcher.SwitchStateInstantly<ChampionIdleState>();
        }
    }

    public override void Update()
    {

    }


    public override void Exit()
    {
        _isStarted = false;
    }

    private async void WaitForFrameEnd()
    {
        await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
        Vector3 stepDirection = Champion.ViewDirection == ViewDirection.Right ? Vector3.left : Vector3.right;
        _moveController.InitializeStepDirection(stepDirection, Animator.GetNextAnimatorStateInfo(Constants.Animator.Layers.BaseLayer).length);
        _isStarted = true;
    }
}