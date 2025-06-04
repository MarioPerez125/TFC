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
        
        [BindProperty]
        public int OrganizerId { get; set; }
        [BindProperty]
        public TournamentDto Tournament { get; set; }

        [BindProperty]
        public string CreateTournamentMessage { get; set; }
        
        [BindProperty]
        public bool CreateTournamentSuccess { get; set; }

        public async Task<IActionResult> OnGetAsync(int organizerId)
        {
            OrganizerId = organizerId;
            // Llama al método para obtener los torneos del organizador
            GetTournamentResponse response = await _tournamentApplication.GetTournamentByOrganizerId(OrganizerId);
            Tournaments = response.Tournament ?? new List<TournamentDto>();
            return Page();
        }

        public async Task<IActionResult> OnPostCreateTournamentAsync()
        {
            if (DateTime.Parse(Tournament.EndDate) <= DateTime.Parse(Tournament.StartDate))
            {
                CreateTournamentSuccess = false;
                CreateTournamentMessage = "La fecha y hora de fin debe ser posterior a la de inicio.";

                // Recarga la lista de torneos para mostrarla aunque haya error
                GetTournamentResponse response = await _tournamentApplication.GetTournamentByOrganizerId(OrganizerId);
                Tournaments = response.Tournament ?? new List<TournamentDto>();

                // Mantén la URL con el parámetro organizerId
                return Page();
            }

            var result = await _tournamentApplication.CreateTournament(Tournament, OrganizerId);

            if (result.IsSuccess)
            {
                CreateTournamentSuccess = true;
                CreateTournamentMessage = "Torneo creado correctamente.";
                return RedirectToPage(new { organizerId = OrganizerId });
            }
            else
            {
                CreateTournamentSuccess = false;
                CreateTournamentMessage = result.Message ?? "Error al crear el torneo.";

                // Recarga la lista de torneos para mostrarla aunque haya error
                GetTournamentResponse response = await _tournamentApplication.GetTournamentByOrganizerId(OrganizerId);
                Tournaments = response.Tournament ?? new List<TournamentDto>();

                // Mantén la URL con el parámetro organizerId
                return Page();
            }
        }
    }
}
