namespace Cloud.Application.ViewModels.Shared;

using System.Globalization;
using Microsoft.AspNetCore.Components;

public class CultureSelectorViewModel : ViewModelBase
{
    private readonly NavigationManager navigation;

    private CultureInfo? culture = Thread.CurrentThread.CurrentCulture;

    public Func<CultureInfo, string>? CultureConverter { get; } = culture => culture.Name;

    public CultureSelectorViewModel(NavigationManager navigation) => this.navigation = navigation;

    public CultureInfo? Culture
    {
        get => this.culture;
        set
        {
            this.SetValue(ref this.culture, value);
            this.RequestCultureChange();
        }
    }

    protected void RequestCultureChange()
    {
        if (string.IsNullOrWhiteSpace(this.culture?.Name))
        {
            return;
        }

        var returnUri = new Uri(this.navigation.Uri).GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
        var queryString = $"?culture={Uri.EscapeDataString(this.culture.Name)}&redirectUri={Uri.EscapeDataString(returnUri)}";

        this.navigation.NavigateTo("Culture/SetCulture" + queryString, forceLoad: true);
    }
}
