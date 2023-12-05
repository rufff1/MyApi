using Common.Constants.Country;
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
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
