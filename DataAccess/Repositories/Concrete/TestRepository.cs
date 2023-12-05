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
    public class TestRepository : Repository<Test>, ITestRepository
    {
        public TestRepository(AppDbContext context) : base(context)
        {
        }
    }
}
