namespace Cloud.Domain.Validations;

using FluentValidation.Results;

public static class FluentValidationFailureHelpers
{
    public static Dictionary<string, string[]> DistinctErrorsByProperty(this ValidationResult? validationResult)
    {
        if (validationResult is null)
        {
            return new Dictionary<string, string[]>();
        }

        var validationFailures = validationResult.Errors;

        return validationFailures
            .GroupBy(validationFailure =>
                validationFailure.PropertyName,
                validationFailure => validationFailure.ErrorMessage,
                (propertyName, validationFailuresByProperty) => new
                {
                    Key = propertyName,
                    Values = validationFailuresByProperty.Distinct().ToArray(),
                })
            .ToDictionary(
                group => group.Key,
                group => group.Values);
    }

    public static Dictionary<string, string[]> DistinctErrorsByProperty(
        this IEnumerable<ValidationResult>? validationResults)
    {
        if (validationResults is null)
        {
            return new Dictionary<string, string[]>();
        }

        return validationResults.Where(validationResult =>
        validationResult is { IsValid: false, Errors: not null, Errors.Count: > 0, })
            .SelectMany(validationResult => validationResult.Errors, (_, validationFailure) => validationFailure)
            .GroupBy(
            validationFailure => validationFailure.PropertyName,
            validationFailure => validationFailure.ErrorMessage,
            (propertyName, validationFailuresByProperty) => new
            {
                Key = propertyName,
                Values = validationFailuresByProperty.Distinct().ToArray(),
            })
            .ToDictionary(
                group => group.Key,
                group => group.Values);
    }
}
