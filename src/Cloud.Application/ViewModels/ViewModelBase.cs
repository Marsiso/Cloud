namespace Cloud.Application.ViewModels;

using System.ComponentModel;
using System.Runtime.CompilerServices;

public class ViewModelBase : INotifyPropertyChanged
{
    public delegate Task ViewModelAfterRenderHandler();
    public delegate Task ViewModelInitializedHandler();
    public delegate Task ViewModelParametersSetHandler();

    private bool busy;

    public bool Busy
    {
        get => this.busy;
        set => this.SetValue(ref this.busy, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public event ViewModelInitializedHandler? ViewModelInitialized;
    public event ViewModelAfterRenderHandler? ViewModelAfterRender;
    public event ViewModelParametersSetHandler? ViewModelParametersSet;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = default!) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    protected void SetValue<TItem>(ref TItem field, TItem value, [CallerMemberName] string propertyName = default!)
    {
        if (EqualityComparer<TItem>.Default.Equals(field, value))
        {
            return;
        }

        field = value;

        this.OnPropertyChanged(propertyName);
    }

    public Task OnViewModelInitialized() => this.ViewModelInitialized?.Invoke() ?? Task.CompletedTask;

    public Task OnViewModelAfterRender() => this.ViewModelAfterRender?.Invoke() ?? Task.CompletedTask;

    public Task OnViewModelParametersSet() => this.ViewModelParametersSet?.Invoke() ?? Task.CompletedTask;
}
