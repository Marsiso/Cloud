using Microsoft.EntityFrameworkCore;

namespace Cloud.Core.Application.Users.Queries;

public record UserWithEmailExistsQuery(string? Email) : IQuery<bool>;

public class UserWithEmailExistsQueryHandler : IQueryHandler<UserWithEmailExistsQuery, bool>
{
    public static readonly Func<DataContext, string, bool> Query = EF.CompileQuery((DataContext context, string email) =>
        context.Users.AsNoTracking()
            .Any(user => user.Email == email));

    private readonly DataContext _context;

    public UserWithEmailExistsQueryHandler(DataContext context)
    {
        _context = context;
    }

    public Task<bool> Handle(UserWithEmailExistsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var exists = false;

        if (string.IsNullOrWhiteSpace(request.Email)) return Task.FromResult(exists);

        exists = Query(_context, request.Email);

        return Task.FromResult(exists);
    }
}
