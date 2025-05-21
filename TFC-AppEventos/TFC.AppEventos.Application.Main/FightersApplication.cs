using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;
using TFC.AppEventos.Application.Interface;
using TFC.AppEventos.Infraestructure.Interface;
using TFC.AppEventos.Infraestructure.Interface.IAuthRepository;
using TFC.AppEventos.Transversal.Utils;

namespace TFC.AppEventos.Application.Main
{
    public class FightersApplication : IFightersApplication
    {
        private readonly IFightersRepository _fightersRepository;

        public FightersApplication(IFightersRepository fightersRepository)
        {
            _fightersRepository = fightersRepository;
        }

        public async Task<GetMyTournamentsAsFighterResponse> GetMyTournamentsAsFighter(int userId)
        {
            if (userId <= 0)
            {
                GetMyTournamentsAsFighterResponse response = new GetMyTournamentsAsFighterResponse
                {
                    IsSuccess = false,
                    Message = "El ID de usuario no puede ser menor o igual a cero."
                };
                return response;
            }
            else
            {
                return await _fightersRepository.GetMyTournamentsAsFighter(userId);
            }
        }

        public async Task<RegisterFighterResponse> RegisterFighter(FightersDTO fighterDto)
        {
            RegisterFighterResponse response = new RegisterFighterResponse();

            try
            {
                // Validaciones básicas
                if (fighterDto == null)
                {
                    response.ResponseCode = ResponseCodes.ERROR_USER_NOTFOUND;
                    throw new Exception("Los datos de autenticación no pueden ser nulos");
                }

                // Llamada al repositorio
                return await _fightersRepository.RegisterFighter(fighterDto);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error durante el registro: {ex.Message}";
                return response;
            }
        }

        public async Task<bool> UnregisterFighter(int id)
        {
            return await _fightersRepository.UnregisterFighter(id);
        }
    }
}
