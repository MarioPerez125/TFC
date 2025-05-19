using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Transversal.Utils;
using System.Text.RegularExpressions;
using TFC.AppEventos.Application.DTO.Responses;

namespace TFC.AppEventos.Application.Main
{
    public class AuthApplication : IAuthApplication
    {
        private readonly IAuthRepository _authRepository;

        public AuthApplication(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<LoginResponse> Login(AuthDto authDto)
        {
            var response = new LoginResponse();

            try
            {
                // Validaciones básicas
                if (authDto == null)
                {
                    response.ResponseCode = ResponseCodes.ERROR_USER_NOTFOUND;
                    throw new Exception("Los datos de autenticación no pueden ser nulos");
                }

                if (string.IsNullOrWhiteSpace(authDto.Username))
                {
                    response.ResponseCode = ResponseCodes.ERROR_USER_NOTFOUND;
                    throw new Exception("El nombre de usuario es requerido");
                }

                if (string.IsNullOrWhiteSpace(authDto.Password))
                {
                    response.ResponseCode = ResponseCodes.ERROR_BAD_PASSWORD;
                    throw new Exception("La contraseña es requerida");
                }

                if (string.IsNullOrWhiteSpace(authDto.Email))
                {
                    response.ResponseCode = ResponseCodes.ERROR_BAD_EMAIL;
                    throw new Exception("El email es requerido");
                }

                // Validación simple de email
                if (!authDto.Email.Contains("@"))
                {
                    response.ResponseCode = ResponseCodes.ERROR_BAD_EMAIL;
                    throw new Exception("El email no es válido");
                }


                // Llamada al repositorio
                return await _authRepository.Login(authDto);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error durante el login: {ex.Message}";
                response.ResponseCode = ResponseCodes.ERROR_NO_CONNECTION;
                return response;
            }
        }

        public async Task<RegisterResponse> Register(AuthDto authDto)
        {
            var response = new RegisterResponse();

            try
            {
                // Validaciones básicas
                if (authDto == null)
                {
                    response.ResponseCode = ResponseCodes.ERROR_USER_NOTFOUND;
                    throw new Exception("Los datos de autenticación no pueden ser nulos");
                }

                if (string.IsNullOrWhiteSpace(authDto.Username))
                {
                    response.ResponseCode = ResponseCodes.ERROR_USER_NOTFOUND;
                    throw new Exception("El nombre de usuario es requerido");
                }

                if (string.IsNullOrWhiteSpace(authDto.Password))
                {
                    response.ResponseCode = ResponseCodes.ERROR_BAD_PASSWORD;
                    throw new Exception("La contraseña es requerida");
                }

                if (authDto.Password.Length < 6)
                {
                    response.ResponseCode = ResponseCodes.ERROR_BAD_PASSWORD;
                    throw new Exception("La contraseña debe tener al menos 6 caracteres");
                }
                // Llamada al repositorio
                return await _authRepository.Register(authDto);
                
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error durante el registro: {ex.Message}";
                return response;
            }
        }
    }
}