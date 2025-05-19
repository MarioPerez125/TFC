using Microsoft.EntityFrameworkCore;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;
using TFC.AppEventos.Application.Main;
using TFC.AppEventos.Database.Context;
using TFC.AppEventos.Domain.Entities;

namespace TFC.AppEventos.Infraestructure.Repository.AuthRepository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public async Task<LoginResponse?> Login(AuthDto authDto)
        {
            LoginResponse response = new LoginResponse();

            User? user = _context.Users
                .FirstOrDefault(u => u.Username == authDto.Username && u.Password == authDto.Password);
            
            if (user != null)
            {
                response.IsSuccess = true;
                response.Message = "Usuario autenticado correctamente";
                response.AuthDto = authDto;
                return response;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Usuario o contraseña incorrectos";
                return response;
            }
        }

        public async Task<RegisterResponse> Register(AuthDto authDto)
        {
            RegisterResponse response = new RegisterResponse();

            User user = new User
            {
                Username = authDto.Username,
                Password = authDto.Password,
                Email = authDto.Email
            };

            await _context.Users.AddAsync(user);
            var result = _context.SaveChangesAsync();
            response.IsSuccess = true;
            response.Message = "Usuario registrado correctamente";
            response.AuthDto = authDto;
            return response;
        }
    }
}