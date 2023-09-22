namespace Cloud.Application.ViewModels.Shared;

public class MainLayoutViewModel : ViewModelBase
{
    private bool isSidebarVisible;

    public bool IsSidebarVisible
    {
        get => this.isSidebarVisible;
        set => this.SetValue(ref this.isSidebarVisible, value);
    }

    public void ToggleSidebar() => this.IsSidebarVisible = !this.IsSidebarVisible;
}
