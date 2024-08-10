using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;

public class HoverHintProvider : AddressableLoader
{
    private HoverHintView _hoverHintView;

    public async Task<HoverHintView> ShowHoverHint(LocalizedString hintText, Vector3 position,
        Quaternion rotation, Transform parent, Vector2 pivot, Vector2 anchorMin, Vector2 anchorMax, Vector3 scale)
    {
        _hoverHintView = await Load<HoverHintView>(Constants.Addressables.Keys.HoverHintView, position, rotation, parent);
        _hoverHintView.Initialize(hintText, pivot, anchorMin, anchorMax, position, scale);
        _hoverHintView.Show();
        return _hoverHintView;
    }

    public void UnloadHoverHint()
    {
        _hoverHintView.Hide();
        _hoverHintView = null;
        UnloadCachedGameObject();
    }
}
