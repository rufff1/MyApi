using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class TeamPosition :BaseEntity
    {
        public string PositionName { get; set; }


        public virtual ICollection<Team> Teams { get; set; }
      
    }
}
