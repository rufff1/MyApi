using Common.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Team.Request
{
    public class TeamUpdateDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public byte Age { get; set; }
        public string Info { get; set; }
        public string? Email { get; set; }



        public int CountryId { get; set; }



        public int PositionId { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
