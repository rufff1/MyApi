using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Constants.Country
{
    public class Country :BaseEntity
    {
 
        public string CountryName { get; set; }


        public virtual ICollection<Country> Countries { get; set; }
    }
}
