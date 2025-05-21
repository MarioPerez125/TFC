using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;

namespace TFC.AppEventos.Application.Interface
{
    public interface ITournamentApplication
    {
        Task<CreateTournamentResponse> CreateTournament(TournamentDto tournamentDto, int organizerId);
        Task<GetAllParticipantsResponse> GetParticipants(int tournamentId);
        Task<GetTournamentResponse> GetTournamentByName(string name);
    }
}
