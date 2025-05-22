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
        /// <summary>
        /// Programa un nuevo combate
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Organizer,Admin")]
        public async Task<ActionResult> Schedule([FromBody] FightDto fightDto)
        {
            OrganizarPeleaResponse response = await _fightApplication.ScheduleFight(fightDto);
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
                return BadRequest(response.Message);
            }
        }

        /// <summary>
        /// Actualiza el resultado de un combate
        /// </summary>
        [Authorize(Roles = "Organizer,Admin")]
        [HttpPut("{id}/result")]
        public async Task<IActionResult> SetResult([FromBody] FightResultDto resultDto)
        {

            return Ok(await _fightApplication.SetAWinner(resultDto));
        }

        /// <summary>
        /// Obtiene los combates de un participante específico
        /// </summary>
        //[HttpGet("fighter/{fighterId}")]
        //public async Task<ActionResult<IEnumerable<FightDto>>> GetByFighter(int fighterId)
        //{
        //}

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
