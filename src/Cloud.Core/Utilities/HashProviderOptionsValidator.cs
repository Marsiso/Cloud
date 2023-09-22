namespace Cloud.Core.Utilities;

using FluentValidation;

public class HashProviderOptionsValidator : AbstractValidator<HashProviderOptions>
{
    public HashProviderOptionsValidator()
    {
        this.RuleFor(options => options.Cycles)
            .GreaterThanOrEqualTo(256_000);

        this.RuleFor(options => options.SaltSize)
            .GreaterThanOrEqualTo(16);

        this.RuleFor(options => options.KeySize)
            .GreaterThanOrEqualTo(32);
    }
}
