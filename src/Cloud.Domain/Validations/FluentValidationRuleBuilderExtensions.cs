namespace Cloud.Domain.Validations;

using FluentValidation;

public static class FluentValidationRuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, string?> URL<T>(this IRuleBuilder<T, string?> ruleBuilder) => ruleBuilder.Must(url =>
    {
        var valid = !string.IsNullOrWhiteSpace(url);

        if (valid)
        {
            valid = Uri.TryCreate(url, UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }

        return valid;
    });
}
