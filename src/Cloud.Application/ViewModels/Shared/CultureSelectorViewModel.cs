using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Cloud.Application.ViewModels.Shared;

public class CultureSelectorViewModel : ViewModelBase
{
    public readonly NavigationManager NavigationManager;

    private CultureInfo _culture = Thread.CurrentThread.CurrentCulture;


    public Func<CultureInfo, string> ConvertFunc = culture => culture.Name;

    public CultureSelectorViewModel(NavigationManager navigationManager)
    {
        NavigationManager = navigationManager;
    }

    public CultureInfo Culture
    {
        get => _culture;
        set
        {
            SetValue(ref _culture, value);
            RequestCultureChange();
        }
    }

    protected void RequestCultureChange()
    {
        if (string.IsNullOrWhiteSpace(_culture.Name)) return;

        var returnUri = new Uri(NavigationManager.Uri).GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
        var queryString = $"?culture={Uri.EscapeDataString(_culture.Name)}&" + $"redirectUri={Uri.EscapeDataString(returnUri)}";

        NavigationManager.NavigateTo("Culture/SetCulture" + queryString, forceLoad: true);
    }
}
