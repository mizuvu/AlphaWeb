using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zord.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext : IDbFactory, IDbSet
{
}
