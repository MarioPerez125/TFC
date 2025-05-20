using Kintech.WebServices.Transversal.Security.TFC.AppEventos.Transversal.Utils;
using Microsoft.EntityFrameworkCore;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;
using TFC.AppEventos.Database.Context;
using TFC.AppEventos.Domain.Entities;
using TFC.AppEventos.Domain.Entities.Enum;
using TFC.AppEventos.Infraestructure.Interface.IAuthRepository;
namespace TFC.AppEventos.Infraestructure.Repository.AuthRepository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;
        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LoginResponse?> Login(AuthDto authDto)
        {
            LoginResponse response = new LoginResponse();

            User? user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == authDto.Username && u.Password == PasswordUtils.PasswordEncoder(authDto.Password));


            if (user != null)
            {
                response.User = new UserDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role
                };
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
                Password = PasswordUtils.PasswordEncoder(authDto.Password),
                Email = authDto.Email,
            };

            User? user2 = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == authDto.Email && u.Password == authDto.Password && u.Username == authDto.Username);
            try
            {
                if (user != null)
                {
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();

                    response.IsSuccess = true;
                    response.Message = "Usuario registrado correctamente";
                    response.AuthDto = authDto;
                    return response;

                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Usuario ya existe";
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Error al registrar el usuario: " + ex.Message;
                return response;
            }
        }
    }
}