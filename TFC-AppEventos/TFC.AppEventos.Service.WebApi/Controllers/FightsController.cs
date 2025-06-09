using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;
using TFC.AppEventos.Application.Interface;

namespace TFC.AppEventos.Service.WebApi.Controllers
{
    [ApiController]
    [Route("api/fights")]

    public class FightsController : ControllerBase
    {
        private readonly IFightApplication _fightApplication;

        public FightsController(IFightApplication fightApplication)
        {
            _fightApplication = fightApplication;
        }

        [HttpPost("schedule-fight")]
        public async Task<ActionResult> Schedule([FromBody] FightDto fightDto)
        {
            OrganizarPeleaResponse response = new OrganizarPeleaResponse();
            response = await _fightApplication.ScheduleFight(fightDto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

        [HttpGet("tournament/{tournamentId}")]
        public async Task<ActionResult<List<FightDto>>> GetByTournament(int tournamentId)
        {
            GetFightsByTournamentResponse response = await _fightApplication.GetFightsByTournament(tournamentId);

            if (response.IsSuccess)
            {
                return Ok(response.Fights);
            }
            else
            {
                return Ok(response.Fights);
            }
        }

        [HttpPut("set-winner")]
        public async Task<IActionResult> SetResult([FromBody] FightResultDto resultDto)
        {

            return Ok(await _fightApplication.SetAWinner(resultDto));
        }

        [HttpGet("fighter/{userId}")]
        public async Task<ActionResult<IEnumerable<FightDto>>> GetByFighter(int userId)
        {
            GetMyFightsResponse response = await _fightApplication.GetMyFights(userId);
            if (response.IsSuccess)
            {
                return Ok(response.Fights);
            }
            else
            {
                return Ok(response.Fights);
            }
        }

        [HttpGet("by-tournament/{tournamentId}")]
        public async Task<ActionResult<GetFightsByTournamentResponse>> GetFightsByTournament(int tournamentId)
        {
            return await _fightApplication.GetFightsByTournament(tournamentId);
        }

        [HttpDelete("{fightId}")]
        public async Task<ActionResult<OrganizarPeleaResponse>> CancelFight(int fightId)
        {
            var result = await _fightApplication.CancelFight(fightId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
