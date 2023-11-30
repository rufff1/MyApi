using Common.Entities;
using DataAccess.Context;
using DataAccess.Repositories.Abstract;
using DataAccess.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Concrete
{
    public class TeamRepository : Repository<Team>, ITeamRepository
    {
        public TeamRepository(AppDbContext context) : base(context)
        {
        }
    }
}
