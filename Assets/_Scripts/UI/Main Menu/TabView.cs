using UnityEngine;
using UnityEngine.UI;

public class TabView : BaseView
{
    [SerializeField] private Button _activeButton;
    [SerializeField] private Selectable _firstSelectable;
    [SerializeField] private Selectable _lastSelectable;

    public bool HasOpenedSelectable;
    public Button ActiveButton => _activeButton;
    public Selectable FirstSelectable => _firstSelectable;
    public Selectable LastSelectable => _lastSelectable;
}