using UnityEngine;
using UnityEngine.UI;

public class HoverHintButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private HoverHintPresenter _hoverHintPresenter;

    public Button Button => _button;
    
    public void TryUnloadHoverHint()
    {
        _hoverHintPresenter.TryUnloadHoverHint();
    }

    public void HideHoverHint()
    {
        _hoverHintPresenter.HideHoverHint();
    }

    public void ShowHoverHint()
    {
        _hoverHintPresenter.ShowHoverHint();
    }
}