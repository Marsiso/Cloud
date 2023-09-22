namespace Cloud.Core.Tests;

using Microsoft.Extensions.Options;

public class OptionsTestWrapper<TOptions> : IOptions<TOptions> where TOptions : class, new()
{
    public TOptions Value { get; }

    public OptionsTestWrapper(TOptions value) => this.Value = value;
}
