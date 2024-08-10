using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HighlightButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Button _button;
    private Color _highlightedColor;
    private Color _unHighlightedColor;

    public Button Button => _button;

    public void Initialize()
    {
        _highlightedColor = _button.colors.highlightedColor;
        _unHighlightedColor = _text.color;
    }

    public void Highlight()
    {
        _text.color = _highlightedColor;
    }

    public void UnHighlight()
    {
        _text.color = _unHighlightedColor;
    }
}