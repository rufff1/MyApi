using Business.DTOs.Common;
using Business.DTOs.Team.Request;
using Business.DTOs.Team.Response;
using Business.DTOs.TeamPosition.Response;
using Business.Helpers;
using Business.Services.Abtraction;
using Business.Services.Concered;
using Common.Entities;
using DataAccess.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _context;

        public TeamController(ITeamService teamService, IEmailService emailService,AppDbContext context)
        {
            _teamService = teamService;
            _emailService = emailService;
            _context = context;

        }

        [HttpPost("SendMailById")]
        public async Task<IActionResult> SendMailAllById(int id,Mailrequest mailrequest)
        {

                   var team =  await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);

                mailrequest.Body = GetHtmlcontent();
                await _emailService.SendEmailAsync(mailrequest, team.Email );



            return Ok();

        }


        [HttpPost("SendMailAll")]
        public async Task<IActionResult> SendMailAll(Mailrequest mailrequest)
        {
            foreach (var item in (await _context.Teams.Where(x=>  x.Email != null).ToListAsync()))
            {
                mailrequest.Body = GetHtmlcontent();
                await _emailService.SendEmailAsync(mailrequest,item.Email);

            }
             
                
                return Ok();
          
        }





        private string GetHtmlcontent()
        {
            string Response = "<div style=\"width:100%;background-color:lightblue;text-align:center;margin:10px\">";
            Response += "<h1>Welcome to Rufat chanell</h1>";
            Response += "<img src=\"https://yt3.googleusercontent.com/v5hyLB4am6E0GZ3y-JXVCxT9g8157eSeNggTZKkWRSfq_B12sCCiZmRhZ4JmRop-nMA18D2IPw=s176-c-k-c0x00ffffff-no-rj\" />";
            Response += "<h2>Izlediyiniz ucun tesekkur edirem</h2>";
            Response += "<a href=\"https://www.youtube.com/watch?v=WHHmiWUqIZA&list=RDWHHmiWUqIZA&start_radio=1\">zehmet olmasa linke girin</a>";
            Response += "<div><h1> Contact us : rufatzg@code.edu.az</h1></div>";
            Response += "</div>";
            return Response;
        }



        #region Documentation
        /// <summary>
        ///  team id parametrinə görə götürülməsi üçün
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<TeamPositionReponseDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        #endregion
        [HttpGet("GetById")]
        public async Task<Response<TeamResponseDTO>> GetAsync(int id)
        {
            return await _teamService.GetAsync(id); 
        }

        #region Documentation
        /// <summary>
        /// team siyahısını götürmək üçün
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<List<TeamPositionReponseDTO>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        #endregion
        [HttpGet("GetAll")]
        public async Task<Response<List<TeamResponseDTO>>> GetAllAsync()
        {
            return await _teamService.GetAllAsync();
        }


        #region Documentation
        /// <summary>
        /// team yaradılması üçün
        /// </summary>
        /// <param name="model"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
        #endregion
        [HttpPost("Create")]
        public async Task<Response> CreateAsync([FromForm] TeamCreateDTO model)
        {
            return await _teamService.CreateAsync(model);
        }

        #region Documentation
        /// <summary>
        /// team redaktə olunması üçün
        /// </summary>
        /// <param name="model"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
        #endregion
        [HttpPut("Update")]
        public async Task<Response> UpdateAsync(int id, [FromForm] TeamUpdateDTO model)
        {
            return await _teamService.UpdateAsync(id,model);
        }



        #region Documentation
        /// <summary>
        ///  team silinməsi  
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Response))]
        #endregion
        [HttpDelete("Delete")]
        public async Task<Response> DeleteAsync(int id)
        {
            return await _teamService.DeleteAsync(id);
        }




    }
}
