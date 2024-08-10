using UnityEngine;
using UnityEngine.UI;

public static class SelectableExtensions
{
    public static void SetSelectableOnUp(this Selectable selectable, Selectable selectableOnUp)
    {
        Navigation navigation = selectable.navigation;
        navigation.mode = Navigation.Mode.Explicit;
        navigation.selectOnUp = selectableOnUp;
        selectable.navigation = navigation;
    }

    public static void SetSelectableOnDown(this Selectable selectable, Selectable selectableOnDown)
    {
        Navigation navigation = selectable.navigation;
        navigation.mode = Navigation.Mode.Explicit;
        navigation.selectOnDown = selectableOnDown;
        selectable.navigation = navigation;
    }

    public static void SetSelectableOnLeft(this Selectable selectable, Selectable selectableOnLeft)
    {
        Navigation navigation = selectable.navigation;
        navigation.mode = Navigation.Mode.Explicit;
        navigation.selectOnLeft = selectableOnLeft;
        selectable.navigation = navigation;
    }

    public static void SetSelectableOnRight(this Selectable selectable, Selectable selectableOnRight)
    {
        Navigation navigation = selectable.navigation;
        navigation.mode = Navigation.Mode.Explicit;
        navigation.selectOnRight = selectableOnRight;
        selectable.navigation = navigation;
    }
}