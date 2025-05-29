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
    [Route("api/tournaments")]
    public class TournamentsController : ControllerBase
    {
        private readonly ITournamentApplication _tournamentsApplication;

        public TournamentsController(ITournamentApplication tournamentsApplication)
        {
            _tournamentsApplication = tournamentsApplication;
        }
        /// <summary>
        /// Obtiene todos los torneos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<TournamentDto>>> GetAll()
        {
            return Ok(await _tournamentsApplication.GetAllTournaments()); // Assuming GetTournamentByName can handle empty name to return all tournaments
        }

        /// <summary>
        /// Obtiene un torneo específico por ID
        /// </summary>
        [HttpGet("{name}")]
        public async Task<ActionResult<List<TournamentDto>>> GetByName(string name)
        {
            GetTournamentResponse response = await _tournamentsApplication.GetTournamentByName(name);

            if (response.IsSuccess)
            {
                return Ok(response.Tournament);
            }
            else
            {
                return Ok(response.Tournament);
            }
        }

        /// <summary>
        /// Crea un nuevo torneo (Requiere rol Organizer o Admin)
        /// </summary>
        [HttpPost("create-tournament")]
        //[Authorize(Roles = nameof(Roles.Organizer) + ","+ nameof(Roles.Admin))]
        public async Task<ActionResult<TournamentDto>> Create([FromBody] TournamentDto tournamentDto)
        {
            CreateTournamentResponse response = new CreateTournamentResponse();

            int organizerId = tournamentDto.OrganizerId;

            response = await _tournamentsApplication.CreateTournament(tournamentDto, organizerId);

            if (response.IsSuccess)
            {
                return Ok(response.Tournament);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

        /// <summary>
        /// Actualiza un torneo existente (Solo organizador o admin)
        /// </summary>
        //[HttpPut("{id}")]
        //[Authorize(Roles = "Organizer,Admin")]
        //public async Task<IActionResult> Update(int id, [FromBody] TournamentDto tournamentDto)
        //{

        //}

        /// <summary>
        /// Elimina un torneo (Solo organizador o admin)
        /// </summary>
        //[HttpDelete("{id}")]
        //[Authorize(Roles = "Organizer,Admin")]
        //public async Task<IActionResult> Delete(int id)
        //{

        //}

        /// <summary>
        /// Obtiene todos los participantes de un torneo
        /// </summary>
        [HttpGet("{tournamentId}/participants")]
        public async Task<ActionResult<IEnumerable<FightersDTO>>> GetParticipants(int tournamentId)
        {
            GetAllParticipantsResponse response = await _tournamentsApplication.GetParticipants(tournamentId);

            if (response.IsSuccess)
            {
                return Ok(response.Participants);
            }
            else
            {
                return Ok(response.Participants);
            }
        }
    }
}
