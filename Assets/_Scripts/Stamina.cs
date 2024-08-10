using Cysharp.Threading.Tasks;
using UnityEngine;

public class Stamina
{
    private readonly ChampionStaminaDisplayer _championStaminaDisplayer;
    private StaminaState _state;
    private const float MaxStamina = 100.0f;
    private const float TimeToStaminaStartRestore = 2.0f;
    private const float StaminaRegenerationValuePerSecond = 20.0f;
    private float _currentTimeToRestoreStamina;
    private float _currentStamina;

    public Stamina(ChampionStaminaDisplayer championStaminaDisplayer)
    {
        _championStaminaDisplayer = championStaminaDisplayer;
        _championStaminaDisplayer.SetStaminaSliderMaxValue(MaxStamina);
        ResetStaminaToMax();
    }

    public void Spend(float value)
    {
        _state = StaminaState.Using;
        _currentTimeToRestoreStamina = 0.0f;
        _currentStamina = Mathf.Clamp(_currentStamina - value, 0.0f, MaxStamina);
        if (IsStaminaEnded())
        {
            _currentStamina = 0.0f;
            _state = StaminaState.Ended;
        }
        _championStaminaDisplayer.SetStaminaSliderValue(_currentStamina);
    }

    public void TryRestore()
    {
        if (IsStaminaMax())
        {
            return;
        }
        if (IsStaminaRestoring() == false && _currentTimeToRestoreStamina > TimeToStaminaStartRestore)
        {
            if (_state == StaminaState.Ended)
            {
                _state = StaminaState.RestoringFromZero;
                Restore();
            }
            else
            {
                _state = StaminaState.RestoringNotFromZero;
                Restore();
            }
        }
        _currentTimeToRestoreStamina += Time.deltaTime;
    }

    public bool IsStaminaEnded()
    {
        return _currentStamina < 0 || Mathf.Approximately(_currentStamina, 0.0f);
    }

    public void ResetStaminaToMax()
    {
        _currentStamina = MaxStamina;
        _championStaminaDisplayer.SetStaminaSliderValue(_currentStamina);
    }

    private async void Restore()
    {
        switch(_state)
        {
            case StaminaState.RestoringFromZero:
                while(IsStaminaMax() == false)
                {
                    _currentStamina = Mathf.Clamp(_currentStamina + StaminaRegenerationValuePerSecond, 0.0f, MaxStamina);
                    _championStaminaDisplayer.SetStaminaSliderValue(_currentStamina);
                    await UniTask.WaitForSeconds(1.0f);
                }
                break;
            case StaminaState.RestoringNotFromZero:
                while (_state != StaminaState.Using || IsStaminaMax())
                {
                    _currentStamina = Mathf.Clamp(_currentStamina + StaminaRegenerationValuePerSecond, 0.0f, MaxStamina);
                    _championStaminaDisplayer.SetStaminaSliderValue(_currentStamina);
                    await UniTask.WaitForSeconds(1.0f);
                }
                break;
        }
    }

    private bool IsStaminaRestoring()
    {
        return _state == StaminaState.RestoringFromZero || _state == StaminaState.RestoringNotFromZero;
    }

    private bool IsStaminaMax() 
    {
        return Mathf.Approximately(_currentStamina, MaxStamina);
    }
}