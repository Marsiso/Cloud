namespace Cloud.Application.ViewModels;

using System.ComponentModel;
using Microsoft.AspNetCore.Components;

public class PageComponentBase<TViewModel> : ComponentBase where TViewModel : ViewModelBase
{
    [Inject] public required TViewModel Model { get; set; }

    public void Dispose() => this.Model.PropertyChanged -= this.OnModelPropertyChanged;

    protected override bool ShouldRender() => !this.Model.Busy;

    protected override Task OnInitializedAsync()
    {
        this.Model.PropertyChanged += this.OnModelPropertyChanged;

        return this.Model.OnViewModelInitialized();
    }

    protected override Task OnParametersSetAsync() => this.Model.OnViewModelParametersSet();

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            return this.Model.OnViewModelAfterRender();
        }

        return base.OnAfterRenderAsync(firstRender);
    }

    private async void OnModelPropertyChanged(object? sender, PropertyChangedEventArgs args) => await this.InvokeAsync(this.StateHasChanged);
}
