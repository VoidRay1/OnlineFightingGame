using System;
using UnityEngine;

public class DropdownTemplate : MonoBehaviour
{
    [SerializeField] private RectTransform _template;
    private const string TemplateName = "Dropdown List";

    public event Action OnDropdownOpened;
    public event Action OnDropdownClosed;

    private void Start()
    {
        if (IsTemplateNameExist())
        {
            OnDropdownOpened?.Invoke();
            GameContext.Instance.WindowCloser.Disable();
        }   
    }

    private void OnDestroy()
    {
        if (IsTemplateNameExist())
        {
            OnDropdownClosed?.Invoke();
            GameContext.Instance.WindowCloser.Enable();
        }
    }

    private bool IsTemplateNameExist()
    {
        return _template.name == TemplateName;
    }
}