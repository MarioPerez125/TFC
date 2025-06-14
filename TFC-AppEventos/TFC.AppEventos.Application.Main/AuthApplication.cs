﻿using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Transversal.Utils;
using System.Text.RegularExpressions;
using TFC.AppEventos.Application.DTO.Responses;
using TFC.AppEventos.Infraestructure.Interface.IAuthRepository;

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

        public async Task<RegisterResponse> Register(RegisterDTO registerDto)
        {
            var response = new RegisterResponse();

            try
            {
                if (registerDto == null)
                {
                    response.ResponseCode = ResponseCodes.ERROR_USER_NOTFOUND;
                    throw new Exception("Los datos de autenticación no pueden ser nulos");
                }

                if (string.IsNullOrWhiteSpace(registerDto.Username))
                {
                    response.ResponseCode = ResponseCodes.ERROR_USER_NOTFOUND;
                    throw new Exception("El nombre de usuario es requerido");
                }

                if (string.IsNullOrWhiteSpace(registerDto.Password))
                {
                    response.ResponseCode = ResponseCodes.ERROR_BAD_PASSWORD;
                    throw new Exception("La contraseña es requerida");
                }

                if (registerDto.Password.Length < 6)
                {
                    response.ResponseCode = ResponseCodes.ERROR_BAD_PASSWORD;
                    throw new Exception("La contraseña debe tener al menos 6 caracteres");
                }

                if (string.IsNullOrWhiteSpace(registerDto.Email))
                {
                    response.ResponseCode = ResponseCodes.ERROR_BAD_EMAIL;
                    throw new Exception("El email es requerido");
                }

                if (!registerDto.Email.Contains("@"))
                {
                    response.ResponseCode = ResponseCodes.ERROR_BAD_EMAIL;
                    throw new Exception("El email no es válido");
                }

                return await _authRepository.Register(registerDto);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error durante el registro: {ex.Message}";
                return response;
            }
        }

        public async Task<ChangeRoleResponse> RegisterAsOrganizer(AuthDto authDto)
        {
            ChangeRoleResponse response = new ChangeRoleResponse();
            try
            {
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

                return await _authRepository.RegisterAsOrganizer(authDto);
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
