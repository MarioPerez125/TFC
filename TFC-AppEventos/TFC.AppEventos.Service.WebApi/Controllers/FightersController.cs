using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;
using TFC.AppEventos.Application.Interface;
using TFC.AppEventos.Domain.Entities.Enum;

namespace TFC.AppEventos.Service.WebApi.Controllers
{
    [ApiController]
    [Route("api/fighters")]
    [Authorize]
    public class FightersController : ControllerBase
    {

        private readonly IFightersApplication _fightersApplication;

        public FightersController(IFightersApplication fightersApplication)
        {
            this._fightersApplication = fightersApplication;
        }


        /// <summary>
        /// Registra un usuario como participante en un torneo (Requiere rol Fighter)
        /// </summary>
        [HttpPost("register-as-fighter")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<FightersDTO>> Register([FromBody] FightersDTO fighterDto)
        {
            RegisterFighterResponse response = await _fightersApplication.RegisterFighter(fighterDto);

            if (response.IsSuccess)
            {
                return Ok(response.Fighter);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

        /// <summary>
        /// Elimina un participante de un torneo (Solo el propio luchador, organizador o admin)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(Roles.Organizer))]
        public async Task<IActionResult> Unregister(int id)
        {
            bool boolean = await _fightersApplication.UnregisterFighter(id);

            if (boolean)
            {
                return Ok("Fighter unregistered successfully.");
            }
            else
            {
                return NotFound("Fighter not found.");
            }
        }

        /// <summary>
        /// Obtiene los torneos en los que participa el usuario actual
        /// </summary>
        [HttpGet("my-tournaments")]
        [Authorize(Roles = nameof(Roles.Fighter))]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetMyTournaments()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            GetMyTournamentsAsFighterResponse response = await _fightersApplication.GetMyTournamentsAsFighter(userId);

            if (response.IsSuccess)
            {
                return Ok(response.Tournaments);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }
    }
}
