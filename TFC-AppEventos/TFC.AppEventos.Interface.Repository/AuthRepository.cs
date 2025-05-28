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
            try
            {
                response.AuthDto = authDto;

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
                    return response;
                }
                else
                {
                    throw new Exception("Usuario o contraseña incorrectos");
                }
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Usuario o contraseña incorrectos";
            }
                return response;
        }

        public async Task<RegisterResponse> Register(RegisterDTO registerDTO)
        {
            RegisterResponse response = new RegisterResponse();

            User user = new User
            {
                Username = registerDTO.Username,
                Password = PasswordUtils.PasswordEncoder(registerDTO.Password),
                Email = registerDTO.Email,
                Name = registerDTO.Name,
                LastName = registerDTO.LastName,
                Phone = registerDTO.Phone,
                BirthDate = registerDTO.BirthDate,
                City = registerDTO.City,
                Country = registerDTO.Country,

            };

            User? user2 = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == registerDTO.Email && u.Password == registerDTO.Password && u.Username == registerDTO.Username);
            try
            {
                if (user != null)
                {
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();

                    response.IsSuccess = true;
                    response.Message = "Usuario registrado correctamente";
                    response.RegisterDTO = registerDTO;
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

        public async Task<RegisterResponse> RegisterAsOrganizer(AuthDto authDto)
        {
            RegisterResponse response = new RegisterResponse();

            

            User? user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == authDto.Username && u.Password == PasswordUtils.PasswordEncoder(authDto.Password));
            user.Role = Roles.Organizer.ToString();

            response.RegisterDTO = new RegisterDTO
            {
                Username = user.Username,
                Password = user.Password,
                Email = user.Email,
                Name = user.Name,
                LastName = user.LastName,
                Phone = user.Phone,
                BirthDate = user.BirthDate,
                City = user.City,
                Country = user.Country
            };

            try
            {
                if (user != null)
                {
                    await _context.SaveChangesAsync();

                    response.IsSuccess = true;
                    response.Message = "Usuario registrado correctamente";
                    return response;

                }
                else
                {
                    throw new Exception("Usuario no existe");
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