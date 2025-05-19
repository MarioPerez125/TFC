//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using TFC.AppEventos.Application.DTO;

//namespace TFC.AppEventos.Service.WebApi.Controllers
//{
//    [ApiController]
//    [Route("api/tournaments")]
//    [Authorize]
//    public class TournamentsController : ControllerBase
//    {
//        /// <summary>
//        /// Obtiene todos los torneos
//        /// </summary>
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetAll()
//        {

//        }

//        /// <summary>
//        /// Obtiene un torneo específico por ID
//        /// </summary>
//        [HttpGet("{id}")]
//        public async Task<ActionResult<TournamentDto>> GetById(int id)
//        {

//        }

//        /// <summary>
//        /// Crea un nuevo torneo (Requiere rol Organizer o Admin)
//        /// </summary>
//        [HttpPost]
//        [Authorize(Roles = "Organizer,Admin")]
//        public async Task<ActionResult<TournamentDto>> Create([FromBody] TournamentDto tournamentDto)
//        {

//        }

//        /// <summary>
//        /// Actualiza un torneo existente (Solo organizador o admin)
//        /// </summary>
//        [HttpPut("{id}")]
//        [Authorize(Roles = "Organizer,Admin")]
//        public async Task<IActionResult> Update(int id, [FromBody] TournamentDto tournamentDto)
//        {

//        }

//        /// <summary>
//        /// Elimina un torneo (Solo organizador o admin)
//        /// </summary>
//        [HttpDelete("{id}")]
//        [Authorize(Roles = "Organizer,Admin")]
//        public async Task<IActionResult> Delete(int id)
//        {

//        }

//        /// <summary>
//        /// Obtiene todos los participantes de un torneo
//        /// </summary>
//        [HttpGet("{tournamentId}/participants")]
//        public async Task<ActionResult<IEnumerable<FighterDto>>> GetParticipants(int tournamentId)
//        {

//        }
//    }
//}
