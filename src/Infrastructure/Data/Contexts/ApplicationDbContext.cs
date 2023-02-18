using Application.Common.Interfaces;
using MediatR;
using System.Reflection;

namespace Infrastructure.Data.Contexts;

public partial class ApplicationDbContext : BaseDbContext, IApplicationDbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUser currentUser,
        IPublisher publisher)
        : base(options, currentUser, publisher)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
