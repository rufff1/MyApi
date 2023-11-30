using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Team.Response
{
    public class TeamResponseDTO
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public byte Age { get; set; }
        public string Info { get; set; }
        public string Image { get; set; }
        public int PositionId { get; set; }
    }
}
