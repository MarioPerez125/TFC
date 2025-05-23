using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;
using TFC.AppEventos.Application.Interface;

namespace TFC.AppEventos.Service.WebApi.Controllers
{
    [ApiController]
    [Route("api/fights")]

    public class FightsController : ControllerBase
    {
        private readonly IFightApplication _fightApplication;

        public FightsController(IFightApplication fightApplication)
        {
            _fightApplication = fightApplication;
        }



        /// <summary>
        /// Programa un nuevo combate
        /// </summary>
        [HttpPost("schedule-fight")]
        //[Authorize(Roles = "Organizer,Admin")]
        public async Task<ActionResult> Schedule([FromBody] FightDto fightDto)
        {
            OrganizarPeleaResponse response = new OrganizarPeleaResponse();
            response = await _fightApplication.ScheduleFight(fightDto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

        /// <summary>
        /// Obtiene todos los combates de un torneo
        /// </summary>
        [HttpGet("tournament/{tournamentId}")]
        public async Task<ActionResult<List<FightDto>>> GetByTournament(int tournamentId)
        {
            GetFightsByTournamentResponse response = await _fightApplication.GetFightsByTournament(tournamentId);

            if (response.IsSuccess)
            {
                return Ok(response.Fights);
            }
            else
            {
                return Ok(response.Fights);
            }
        }

        /// <summary>
        /// Actualiza el resultado de un combate
        /// </summary>
        //[Authorize(Roles = "Organizer,Admin")]
        [HttpPut("set-winner")]
        public async Task<IActionResult> SetResult([FromBody] FightResultDto resultDto)
        {

            return Ok(await _fightApplication.SetAWinner(resultDto));
        }

        /// <summary>
        /// Obtiene los combates de un participante específico
        /// </summary>
        [HttpGet("fighter/{userId}")]
        public async Task<ActionResult<IEnumerable<FightDto>>> GetByFighter(int userId)
        {
            GetMyFightsResponse response = await _fightApplication.GetMyFights(userId);
            if (response.IsSuccess)
            {
                return Ok(response.Fights);
            }
            else
            {
                return Ok(response.Fights);
            }
        }

        /// <summary>
        /// Cancela un combate programado
        /// </summary>
        //[HttpPut("{id}/cancel")]
        //[Authorize(Roles = "Organizer,Admin")]
        //public async Task<IActionResult> Cancel(int id)
        //{
        //}
    }
}
