using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;
using TFC.AppEventos.Application.Interface;
using TFC.AppEventos.Infraestructure.Interface;

namespace TFC.AppEventos.Application.Main
{
    public class FightApplication : IFightApplication
    {
        private readonly IFightRepository _fightRepository;
        public FightApplication(IFightRepository fightRepository)
        {
            _fightRepository = fightRepository;
        }

        public async Task<GetFightsByTournamentResponse> GetFightsByTournament(int tournamentId)
        {
            GetFightsByTournamentResponse response = new GetFightsByTournamentResponse();
            try
            {
                if (tournamentId <= 0)
                {
                    throw new Exception("El ID del torneo no puede ser menor o igual a cero");
                }
                return await _fightRepository.GetFightsByTournament(tournamentId);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error al obtener los combates: {ex.Message}";
                return response;
            }
        }

        public async Task<OrganizarPeleaResponse> ScheduleFight(FightDto fightDto)
        {
            OrganizarPeleaResponse response = new OrganizarPeleaResponse();
            try
            {
                if (fightDto == null )
                    {
                    throw new Exception("Los datos de la pelea no pueden ser nulos");

                }

                return await _fightRepository.ScheduleFight(fightDto);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error durante la programación de la pelea: {ex.Message}";
                return response;
            }
        }

        public async Task<object?> SetAWinner(FightResultDto resultDto)
        {
            if (resultDto == null)
            {
                throw new ArgumentNullException(nameof(resultDto), "El resultado de la pelea no puede ser nulo");
            }
            return await _fightRepository.SetAWinner(resultDto);
        }
    }
}
