using Cloud.Domain.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Cloud.Core.Application.Users.Queries;

public record GetUserQuery(int ID) : IQuery<User?>;

public class GetUserQueryHandler : IQueryHandler<GetUserQuery, User?>
{
    public static readonly Func<DataContext, int, User?> Query = EF.CompileQuery((DataContext context, int id) =>
        context.Users.AsNoTracking()
            .SingleOrDefault(user => user.ID == id));

    private readonly DataContext _context;

    public GetUserQueryHandler(DataContext context)
    {
        _context = context;
    }

    public Task<User?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        if (request.ID < 1) return Task.FromResult<User?>(default);

        var originalUser = Query(_context, request.ID);

        return Task.FromResult(originalUser);
    }
}
