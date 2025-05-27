using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;
using TFC.AppEventos.Application.Main;
using TFC.AppEventos.Transversal.Utils;

namespace TFC.AppEventos.Service.WebApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthApplication _authApplication;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthApplication authApplication, IConfiguration configuration)
        {
            _authApplication = authApplication;
            _configuration = configuration;
        }

        /// <summary>
        /// Registra un nuevo usuario
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult<RegisterDTO>> Register([FromBody] RegisterDTO registerDTO)
        {
            var response = await _authApplication.Register(registerDTO);

            if (response.IsSuccess)
            {
                return Ok(response.RegisterDTO);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

        /// <summary>
        /// Inicia sesión y obtiene un token JWT
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] AuthDto authDto)
        {
            var loginResponse = await _authApplication.Login(authDto);

            if (loginResponse.IsSuccess)
            {
                // Generar token JWT
                var token = GenerateJwtToken(loginResponse.User);

                var response = new LoginResponse
                {
                    IsSuccess = true,
                    Message = "Autenticación exitosa",
                    ResponseCode = ResponseCodes.OK,
                    AuthDto = authDto,
                    User = loginResponse.User,
                    Token = token,
                    TokenExpiration = DateTime.UtcNow.AddMonths(Convert.ToInt16(_configuration["Jwt:ExpireMonths"]))
                };

                return Ok(response);
            }
            else
            {
                return BadRequest(loginResponse.Message);
            }


        }

        [HttpPost("register-as-organizer")]
        public async Task<ActionResult<AuthDto>> RegisterAsOrganizer([FromBody] RegisterDTO registerDTO)
        {
            RegisterResponse response = await _authApplication.RegisterAsOrganizer(registerDTO);

            if (response.IsSuccess)
            {
                return Ok(response.RegisterDTO);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

        private string GenerateJwtToken(UserDto user)
        {
            Console.WriteLine($"JWT Key: {_configuration["Jwt:Key"]}");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Crear claims con los nombres correctos
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("UserId", user.UserId.ToString()),
        new Claim("username", user.Username),
        
        // Claim de role con el formato correcto
        new Claim(ClaimTypes.Role, user.Role) // Esto generará "role" en lugar del nombre largo
    };

            // Si necesitas mantener compatibilidad con ambos formatos:

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}