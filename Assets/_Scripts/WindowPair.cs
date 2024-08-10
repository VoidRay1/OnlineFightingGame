public class WindowPair
{
    private BaseView _windowToHide;
    private BaseView _windowToShow;

    public BaseView WindowToHide => _windowToHide;
    public BaseView WindowToShow => _windowToShow;

    public WindowPair(BaseView windowToHide, BaseView windowToShow)
    {
        _windowToHide = windowToHide;
        _windowToShow = windowToShow;
    }

    public WindowPair(BaseView windowToHide) 
    {
        _windowToHide = windowToHide;
    }
}