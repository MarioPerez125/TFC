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

        [HttpGet]
        public async Task<ActionResult<List<TournamentDto>>> GetAll()
        {
            return Ok(await _tournamentsApplication.GetAllTournaments()); 
        }

        [HttpGet("organizer/{organizerId}")]
        public async Task<ActionResult<GetTournamentResponse>> GetTournamentsByOrganizer(int organizerId)
        {
            return await _tournamentsApplication.GetTournamentByOrganizerId(organizerId);
        }

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

        [HttpPost("participar-en-torneo")]
        public async Task<ActionResult> ParticiparEnTorneo([FromBody] ParticipantesDTO participantesDTO)
        {
            AñadirParticipanteResponse añadirParticipanteResponse = await _tournamentsApplication.ParticiparEnTorneo(participantesDTO);
            if (añadirParticipanteResponse.IsSuccess)
            {
                return Ok(añadirParticipanteResponse.Message);
            }
            else
            {
                return BadRequest(añadirParticipanteResponse.Message);
            }
        }

        [HttpPost("create-tournament")]
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

        //[HttpPut("{id}")]
        //[Authorize(Roles = "Organizer,Admin")]
        //public async Task<IActionResult> Update(int id, [FromBody] TournamentDto tournamentDto)
        //{

        //}

        //[HttpDelete("{id}")]
        //[Authorize(Roles = "Organizer,Admin")]
        //public async Task<IActionResult> Delete(int id)
        //{

        //}

        [HttpGet("{tournamentId}/peleadores")]
        public async Task<ActionResult<IEnumerable<FightersDTO>>> GetPeleadores(int tournamentId)
        {
            GetAllParticipantsResponse response = await _tournamentsApplication.GetPeleadores(tournamentId);

            if (response.IsSuccess)
            {
                return Ok(response.Participants);
            }
            else
            {
                return Ok(response.Participants);
            }
        }

        [HttpGet("{tournamentId}/participants-para-pelear")]
        public async Task<ActionResult<List<ParticipantesDTO>>> GetParticipantesParaPelear(int tournamentId)
        {
            ObtenerParticipantesResponse response = await _tournamentsApplication.GetParticipantesParaPelear(tournamentId);

            if (response.IsSuccess)
            {
                return Ok(response.Participantes);
            }
            else
            {
                return Ok(response.Participantes);
            }
        }
    }
}
