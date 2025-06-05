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

        public async Task<List<TournamentDto>?> GetAllTournaments()
        {
            return await _tournamentRepository.GetAllTournaments();
        }

        public async Task<ObtenerParticipantesResponse> GetParticipantesParaPelear(int tournamentId)
        {
            ObtenerParticipantesResponse response = new ObtenerParticipantesResponse();
            try
            {
                if (tournamentId <= 0)
                {
                    throw new Exception("El ID del torneo no puede ser menor o igual a cero");
                }
                return await _tournamentRepository.GetParticipantesParaPelear(tournamentId);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = "Error al obtener los participantes para pelear: " + e.Message;
                return response;
            }
        }

        public async Task<GetAllParticipantsResponse> GetPeleadores(int tournamentId)
        {
            GetAllParticipantsResponse response = new GetAllParticipantsResponse();
            try
            {
                if (tournamentId <= 0)
                {
                    throw new Exception("El ID del torneo no puede ser menor o igual a cero");
                }
                return await _tournamentRepository.GetPeleadores(tournamentId);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = "Error al obtener los participantes: " + e.Message;
                return response;
            }
        }

        public Task<TournamentDto> GetTournamentById(int tournamentId)
        {
            return _tournamentRepository.GetTournamentById(tournamentId);
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

        public Task<GetTournamentResponse> GetTournamentByOrganizerId(int name)
        {
            GetTournamentResponse response = new GetTournamentResponse();
            try
            {
                if (name <= 0)
                {
                    throw new Exception("El ID del organizador no puede ser menor o igual a cero");
                }
                return _tournamentRepository.GetTournamentByOrganizerId(name);

            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = "Error al obtener el torneo por ID del organizador: " + e.Message;
                return Task.FromResult(response);
            }
        }

        public async Task<AñadirParticipanteResponse> ParticiparEnTorneo(ParticipantesDTO participantesDTO)
        {
            AñadirParticipanteResponse response = new AñadirParticipanteResponse();
            try
            {
                if (participantesDTO == null)
                {
                    throw new Exception("Los datos del participante no pueden ser nulos");
                }

                return await _tournamentRepository.ParticiparEnTorneo(participantesDTO);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = "Error al participar en el torneo: " + e.Message;
                return response;
            }
        }
    }
}
