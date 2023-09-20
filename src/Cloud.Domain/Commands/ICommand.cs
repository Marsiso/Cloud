using MediatR;

namespace Cloud.Domain.Commands;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
