using Microsoft.AspNetCore.Mvc;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;
using TFC.AppEventos.Application.Main;

namespace TFC.AppEventos.Service.WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthApplication _authApplication;

        /// <summary>
        /// Registra un nuevo usuario
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] AuthDto authDto)
        {
            RegisterResponse registerResponse = await _authApplication.Register(authDto);
            if (registerResponse.IsSuccess)
            {
                return Ok(registerResponse);
            }
            else
            {
                return BadRequest(registerResponse);
            }
        }

        /// <summary>
        /// Inicia sesión y obtiene un token JWT
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] AuthDto authDto)
        {
            LoginResponse loginResponse = await _authApplication.Login(authDto);
            if (loginResponse.IsSuccess)
            {
                return Ok(loginResponse);
            }
            else
            {
                return BadRequest(loginResponse);
            }
        }
    }
}
