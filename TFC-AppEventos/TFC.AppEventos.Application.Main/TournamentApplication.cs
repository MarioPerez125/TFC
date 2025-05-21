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
    public class TournamentApplication : ITournamentApplication
    {
        private readonly ITournamentRepository _tournamentRepository;

        public TournamentApplication(ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public async Task<CreateTournamentResponse> CreateTournament(TournamentDto tournamentDto, int organizerId)
        {
            CreateTournamentResponse response = new CreateTournamentResponse();
            try
            {

                if (tournamentDto == null)
                {
                    throw new Exception("Los datos del torneo no pueden ser nulos");
                }
                if (string.IsNullOrEmpty(tournamentDto.location))
                {
                    throw new Exception("El nombre del torneo no puede ser nulo o vacío");
                }
                if (tournamentDto.StartDate == DateTime.MinValue)
                {
                    throw new Exception("La fecha de inicio no puede ser nula");
                }
                if (tournamentDto.StartDate >= tournamentDto.EndDate)
                {
                    throw new Exception("La fecha de inicio debe ser anterior a la fecha de fin");
                }
                if (tournamentDto.SportType == null)
                {
                    throw new Exception("El tipo de deporte no puede ser nulo");
                }
                // Llamada al repositorio
                return await _tournamentRepository.CreateTournament(tournamentDto, organizerId);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = "Error al crear el torneo: " + e.Message;
                return response;
            }
        }

        public async Task<GetAllParticipantsResponse> GetParticipants(int tournamentId)
        {
            GetAllParticipantsResponse response = new GetAllParticipantsResponse();
            try
            {
                if (tournamentId <= 0)
                {
                    throw new Exception("El ID del torneo no puede ser menor o igual a cero");
                }
                return await _tournamentRepository.GetParticipants(tournamentId);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = "Error al obtener los participantes: " + e.Message;
                return response;
            }
        }

        public async Task<GetTournamentResponse> GetTournamentByName(string name)
        {
            GetTournamentResponse response = new GetTournamentResponse();
            try
            {

                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("El nombre del torneo no puede ser nulo o vacío");
                }

                return await _tournamentRepository.GetTournamentByName(name);
            }


            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = "Error al obtener el torneo: " + e.Message;
                return response;
            }
        }
    }
}
