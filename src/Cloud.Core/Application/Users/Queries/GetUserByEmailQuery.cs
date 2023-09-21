using Cloud.Domain.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Cloud.Core.Application.Users.Queries;

public record GetUserByEmailQuery(string? Email) : IQuery<User?>;

public class GetUserByEmailQueryHandler : IQueryHandler<GetUserByEmailQuery, User?>
{
    public static readonly Func<DataContext, string, User?> Query = EF.CompileQuery((DataContext context, string email) =>
        context.Users.AsNoTracking()
            .SingleOrDefault(user => user.Email == email));

    private readonly DataContext _context;

    public GetUserByEmailQueryHandler(DataContext context)
    {
        _context = context;
    }

    public Task<User?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        if (string.IsNullOrWhiteSpace(request.Email)) return Task.FromResult<User?>(default);

        var originalUser = Query(_context, request.Email);

        return Task.FromResult(originalUser);
    }
}
