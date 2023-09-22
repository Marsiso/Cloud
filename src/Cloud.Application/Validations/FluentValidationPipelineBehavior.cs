namespace Cloud.Application.Validations;

using Cloud.Domain.Exceptions;
using Cloud.Domain.Validations;
using FluentValidation;
using MediatR;

public class FluentValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> validators;

    public FluentValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        ArgumentNullException.ThrowIfNull(validators);

        this.validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!this.validators.Any())
        {
            return await next();
        }

        var validationContext = new ValidationContext<TRequest>(request);

        var validationResults = this.validators.Select(validator => validator.Validate(validationContext));

        var validationFailures = validationResults.DistinctErrorsByProperty();

        if (validationFailures.Count > 0)
        {
            throw new RequestValidationException(validationFailures);
        }

        return await next();
    }
}
