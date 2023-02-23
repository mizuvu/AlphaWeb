using Zord.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext : IDbFactory, IDbSet
{
}
