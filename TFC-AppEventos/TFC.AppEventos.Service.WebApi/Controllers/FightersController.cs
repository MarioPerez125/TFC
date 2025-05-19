//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using TFC.AppEventos.Application.DTO;

//namespace TFC.AppEventos.Service.WebApi.Controllers
//{
//    [ApiController]
//    [Route("api/fighters")]
//    [Authorize]
//    public class FightersController : ControllerBase
//    {
//        /// <summary>
//        /// Registra un usuario como participante en un torneo (Requiere rol Fighter)
//        /// </summary>
//        [HttpPost]
//        [Authorize(Roles = "Fighter")]
//        public async Task<ActionResult<FighterDto>> Register([FromBody] FighterDto fighterDto)
//        {

//        }

//        /// <summary>
//        /// Elimina un participante de un torneo (Solo el propio luchador, organizador o admin)
//        /// </summary>
//        [HttpDelete("{id}")]
//        [Authorize(Roles = "Fighter,Organizer,Admin")]
//        public async Task<IActionResult> Unregister(int id)
//        {

//        }

//        /// <summary>
//        /// Obtiene los torneos en los que participa el usuario actual
//        /// </summary>
//        [HttpGet("my-tournaments")]
//        [Authorize(Roles = "Fighter")]
//        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetMyTournaments()
//        {

//        }
//    }
//}
