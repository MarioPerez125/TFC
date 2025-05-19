//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using TFC.AppEventos.Application.DTO;
//namespace TFC.AppEventos.Service.WebApi.Controllers
//{
//    [ApiController]
//    [Route("api/results")]
//    [Authorize]
//    public class ResultsController : ControllerBase
//    {
//        /// <summary>
//        /// Obtiene los resultados de un torneo
//        /// </summary>
//        [HttpGet("tournament/{tournamentId}")]
//        public async Task<ActionResult<IEnumerable<FightResultDto>>> GetTournamentResults(int tournamentId) { }

//        /// <summary>
//        /// Obtiene el historial de combates de un participante
//        /// </summary>
//        [HttpGet("fighter/{fighterId}")]
//        public async Task<ActionResult<IEnumerable<FightResultDto>>> GetFighterHistory(int fighterId) { }
//    }
//}
