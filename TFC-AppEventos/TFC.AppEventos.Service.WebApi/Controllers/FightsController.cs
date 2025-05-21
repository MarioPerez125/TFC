using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;
using TFC.AppEventos.Application.Interface;

namespace TFC.AppEventos.Service.WebApi.Controllers
{
    [ApiController]
    [Route("api/fights")]
    [Authorize(Roles = "Organizer,Admin")]
    public class FightsController : ControllerBase
    {
        private readonly IFightApplication _fightApplication;
        /// <summary>
        /// Programa un nuevo combate
        /// </summary>
        [HttpPost]
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
        //[HttpGet("tournament/{tournamentId}")]
        //public async Task<ActionResult<IEnumerable<FightDto>>> GetByTournament(int tournamentId)
        //{
        //}

        /// <summary>
        /// Actualiza el resultado de un combate
        /// </summary>
        //[HttpPut("{id}/result")]
        //public async Task<IActionResult> SetResult(int id, [FromBody] FightResultDto resultDto)
        //{
        //}

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
        //public async Task<IActionResult> Cancel(int id)
        //{
        //}
    }
}
