//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using TFC.AppEventos.Application.DTO;

//namespace TFC.AppEventos.Service.WebApi.Controllers
//{
//    [ApiController]
//    [Route("api/fights")]
//    [Authorize(Roles = "Organizer,Admin")]
//    public class FightsController : ControllerBase
//    {
//        /// <summary>
//        /// Programa un nuevo combate
//        /// </summary>
//        [HttpPost]
//        public async Task<ActionResult<FightDto>> Schedule([FromBody] FightDto fightDto)
//        {

//        }

//        /// <summary>
//        /// Obtiene todos los combates de un torneo
//        /// </summary>
//        [HttpGet("tournament/{tournamentId}")]
//        public async Task<ActionResult<IEnumerable<FightDto>>> GetByTournament(int tournamentId)
//        {
//        }

//        /// <summary>
//        /// Actualiza el resultado de un combate
//        /// </summary>
//        [HttpPut("{id}/result")]
//        public async Task<IActionResult> SetResult(int id, [FromBody] FightResultDto resultDto)
//        {
//        }

//        /// <summary>
//        /// Obtiene los combates de un participante específico
//        /// </summary>
//        [HttpGet("fighter/{fighterId}")]
//        public async Task<ActionResult<IEnumerable<FightDto>>> GetByFighter(int fighterId)
//        {
//        }

//        /// <summary>
//        /// Cancela un combate programado
//        /// </summary>
//        [HttpPut("{id}/cancel")]
//        public async Task<IActionResult> Cancel(int id)
//        {
//        }
//    }
//}
