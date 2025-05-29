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
        //[Authorize(Roles = "User")]
        public async Task<ActionResult<UserDto>> Register([FromBody] FightersDTO fighterDto)
        {
            ChangeRoleResponse response = await _fightersApplication.RegisterFighter(fighterDto);

            if (response.IsSuccess)
            {
                return Ok(response.User);
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
        //[Authorize(Roles = nameof(Roles.Organizer))]
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
        [HttpGet("my-tournaments/{userId}")]
        //[Authorize(Roles = nameof(Roles.Fighter))]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetMyTournaments(int userId)
        {

            GetMyTournamentsAsFighterResponse response = await _fightersApplication.GetMyTournamentsAsFighter(userId);

            if (response.IsSuccess)
            {
                return Ok(response.Tournaments);
            }
            else
            {
                return Ok(response.Tournaments);
            }
        }

        [HttpGet("user-fighter-info/{userId}")]
        //[Authorize(Roles = nameof(Roles.Fighter))]
        public async Task<ActionResult<FightersDTO>> GetFighterInfo(int userId)
        {
            GetFighterInfoResponse response = await _fightersApplication.GetFighterInfo(userId);
            if (response.IsSuccess)
            {
                return Ok(response.FighterInfo.Fighter);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

        [HttpGet("user-fighter-list")]
        public async Task<ActionResult<List<FighterForFriendList>>> GetFighterList()
        {
            GetUserFighterInfoListResponse response = await _fightersApplication.GetFighterList();
            if (response.IsSuccess)
            {
                return Ok(response.fighterList);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }
    }
}
