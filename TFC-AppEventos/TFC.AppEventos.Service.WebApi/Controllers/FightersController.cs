using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;
using TFC.AppEventos.Application.Interface;
using TFC.AppEventos.Domain.Entities.Enum;

namespace TFC.AppEventos.Service.WebApi.Controllers
{
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
        public async Task<ActionResult<FighterDto>> Register([FromBody] FighterDto fighterDto)
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

        [HttpGet("me")]
        [Authorize]
        public IActionResult GetMe()
        {
            try
            {
                var username = User.Identity?.Name;
                var roles = User.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value);

                return Ok(new { username, roles });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, stack = ex.StackTrace });
            }
        }

        /// <summary>
        /// Elimina un participante de un torneo (Solo el propio luchador, organizador o admin)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(Roles.User))]
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
        [Authorize(Roles = "Fighter")]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetMyTournaments()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Console.WriteLine($"UserId: {userId}");

            GetMyTournamentsResponse response = await _fightersApplication.GetMyTournaments(userId);

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
