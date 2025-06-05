using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;

namespace TFC.AppEventos.Infraestructure.Interface
{
    public interface ITournamentRepository
    {
        Task<CreateTournamentResponse> CreateTournament(TournamentDto tournamentDto, int organizerId);
        Task<List<TournamentDto>?> GetAllTournaments();
        Task<ObtenerParticipantesResponse> GetParticipantesParaPelear(int tournamentId);
        Task<GetAllParticipantsResponse> GetPeleadores(int tournamentId);
        Task<TournamentDto> GetTournamentById(int tournamentId);
        Task<GetTournamentResponse> GetTournamentByName(string name);
        Task<GetTournamentResponse> GetTournamentByOrganizerId(int name);
        Task<AñadirParticipanteResponse> ParticiparEnTorneo(ParticipantesDTO participantesDTO);
    }
}
