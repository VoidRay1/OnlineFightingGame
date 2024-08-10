using System.Collections.Generic;

public struct ChangedSettingValue<T>
{
    private T _newValue;
    public T OriginalValue;
    public T NewValue
    {
        set
        {
            _newValue = value;
            if (EqualityComparer<T>.Default.Equals(_newValue, OriginalValue))
            {
                IsChanged = false;
            }
            else
            {
                IsChanged = true;
            }
        }
        get 
        {
            return _newValue; 
        }
    }
    public bool IsChanged { get; private set; }

    public ChangedSettingValue(T value)
    {
        OriginalValue = value;
        IsChanged = false;
        _newValue = value;
    }
}