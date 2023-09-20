namespace Cloud.Application.ViewModels.Shared;

public class MainLayoutViewModel : ViewModelBase
{
    private bool _isSidebarVisible;

    public bool IsSidebarVisible
    {
        get => _isSidebarVisible;
        set => SetValue(ref _isSidebarVisible, value);
    }

    public void ToggleSidebar()
    {
        IsSidebarVisible = !IsSidebarVisible;
    }
}
