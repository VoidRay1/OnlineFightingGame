using System.Collections.Generic;
using UnityEngine.UI;
using static UnityEngine.UI.Selectable;

public class MultiTabsView : BaseView
{
    private List<TabView> _tabs;
    private TabView _selectedTab;
    private Button _activeButton;

    public Button ActiveButton => _activeButton;

    public void SetTabs(List<TabView> tabs)
    {
        _tabs = tabs;
    }

    public void HideTabs()
    {
        foreach (var tab in _tabs)
        {
            tab.Hide();
        }
    }

    public void ShowTabWithNavigationOnDown(TabView tab)
    {
        ShowTab(tab);
        SetTabsNavigationOnDown();
        tab.Show();
    }

    public void ShowTabWithoutNavigationOnDown(TabView tab)
    {
        ShowTab(tab);
        tab.Show();
    } 

    private void ShowTab(TabView tab)
    {
        HighlightActiveButton(tab.ActiveButton);
        if (IsWindowExists())
        {
            //_selectedTab.ActiveButton.SetSelectableOnDown(null);
            _selectedTab.Hide();
        }
        _selectedTab = tab;
    }

    private void SetTabsNavigationOnDown()
    {
        foreach (var tab in _tabs)
        {
            tab.ActiveButton.SetSelectableOnDown(_selectedTab.FirstSelectable);
        }
    }

    private void HighlightActiveButton(Button buttonToHiglight)
    {
        buttonToHiglight.Select();
        if (IsActiveButtonExists())
        {
            _activeButton.transition = Transition.ColorTint;
        }
        _activeButton = buttonToHiglight;
        buttonToHiglight.transition = Transition.None;
    }

    private bool IsWindowExists()
    {
        return _selectedTab != null;
    }

    private bool IsActiveButtonExists()
    {
        return _activeButton != null;
    }
}