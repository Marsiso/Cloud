using Microsoft.AspNetCore.Components;

namespace Cloud.Application.ViewModels;

public class MainLayoutComponent : LayoutComponentBase
{
	public bool IsSidebarVisible { get; set; }

	public void ToggleSidebar()
	{
		IsSidebarVisible = !IsSidebarVisible;
	}
}
