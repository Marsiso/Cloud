using AutoMapper;
using Cloud.Domain.Application.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cloud.Core.Application.Users.Commands;

public record CreateUserCommand(string? Email, string? GivenName, string? FamilyName, string? Password, string? ProfilePhotoUrl) : ICommand<Unit>;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Unit>
{
    private readonly DataContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(DataContext context, IPasswordHasher passwordHasher, IMapper mapper, ILogger<CreateUserCommandHandler> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
        _logger = logger;
    }

    public Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var user = _mapper.Map<User>(request);

        (user.Password, user.PasswordSalt) = _passwordHasher.HashPassword(request.Password);

        _ = _context.Users.Add(user);
        _ = _context.SaveChanges();

        return Unit.Task;
    }
}
