using System;
using UnityEngine;

public abstract class BaseView : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    public event Action OnViewShown;
    public event Action OnViewHidden;
    public Canvas Canvas => _canvas;

    public virtual void Show()
    {
        Canvas.enabled = true;
        OnViewShown?.Invoke();
    }

    public virtual void Hide()
    {
        Canvas.enabled = false;
        OnViewHidden?.Invoke();
    }
}