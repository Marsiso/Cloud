using MediatR;

namespace Cloud.Domain.Queries;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
