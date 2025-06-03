using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TFC.AppEventos.Application.Interface;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;

namespace OrganizerWeb.Pages
{
    public class MyTournamentsModel : PageModel
    {
        private readonly ITournamentApplication _tournamentApplication;

        public MyTournamentsModel(ITournamentApplication tournamentApplication)
        {
            _tournamentApplication = tournamentApplication;
        }

        public List<TournamentDto> Tournaments { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int organizerId)
        {
            // Llama al método para obtener los torneos del organizador
            GetTournamentResponse response = await _tournamentApplication.GetTournamentByOrganizerId(organizerId);
            Tournaments = response.Tournament ?? new List<TournamentDto>();
            return Page();
        }
    }
}
