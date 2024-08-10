using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class WindowCloser
{
    private readonly Stack<WindowPair> _windowPairs = new Stack<WindowPair>();
    private readonly GameControls.WindowActions _windowActions;
    private bool _isEnabled;

    public event Action OnCloseWindowTrigger;

    public bool IsEnabled => _isEnabled;

    public WindowCloser(GameControls.WindowActions windowActions)
    {
        _windowActions = windowActions;
        _windowActions.Close.started += TryHideWindow;
        _isEnabled = true;
    }

    public void Enable()
    {
        _isEnabled = true;
    }

    public void Disable() 
    {
        _isEnabled = false; 
    }

    public void AddWindowToHide(BaseView windowToHide)
    {
        windowToHide.OnViewHidden += RemoveWindowsPair;
        _windowPairs.Push(new WindowPair(windowToHide));
    }

    public void AddPairWindow(BaseView windowToHide, BaseView windowToShow)
    {
        windowToHide.OnViewHidden += RemoveWindowsPair;
        _windowPairs.Push(new WindowPair(windowToHide, windowToShow));
    }

    private void RemoveWindowsPair()
    {
        if (_windowPairs.TryPeek(out WindowPair windowPair))
        {
            windowPair.WindowToHide.OnViewHidden -= RemoveWindowsPair;
            if (windowPair.WindowToShow != null)
            {
                windowPair.WindowToShow.Show();
            }
            _windowPairs.Pop();
        }
    }

    public void TryHideWindow()
    {
        if (_windowPairs.TryPeek(out WindowPair windowPair))
        {
            windowPair.WindowToHide.OnViewHidden -= RemoveWindowsPair;
            if (windowPair.WindowToShow == null)
            {
                windowPair.WindowToHide.Hide();
            }
            else
            {
                windowPair.WindowToHide.Hide();
                windowPair.WindowToShow.Show();
            }
            _windowPairs.Pop();
        }
    }

    private void TryHideWindow(InputAction.CallbackContext obj)
    {
        OnCloseWindowTrigger?.Invoke();
        if (_isEnabled == false)
        {
            return;
        }
        TryHideWindow();
    }

    ~WindowCloser()
    {
        _windowActions.Close.started -= TryHideWindow;
    }
}