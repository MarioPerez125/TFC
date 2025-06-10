using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;

namespace OrganizerWeb.Pages
{
    [Authorize(Roles = "Organizer")]
    public class MyTournamentsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MyTournamentsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
            var client = _httpClientFactory.CreateClient("Api");
            var response = await client.GetAsync($"api/tournaments/organizer/{OrganizerId}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GetTournamentResponse>();
                Tournaments = result?.Tournament ?? new List<TournamentDto>();

                Tournaments = Tournaments
                    .OrderByDescending(t => DateTime.TryParse(t.StartDate, out var dt) ? dt : DateTime.MinValue)
                    .ToList();
            }
            else
            {
                Tournaments = new List<TournamentDto>();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostCreateTournamentAsync()
        {
            if (DateTime.Parse(Tournament.EndDate) <= DateTime.Parse(Tournament.StartDate))
            {
                CreateTournamentSuccess = false;
                CreateTournamentMessage = "La fecha y hora de fin debe ser posterior a la de inicio.";
                await OnGetAsync(OrganizerId);
                return Page();
            }

            Tournament.OrganizerId = OrganizerId; 
            var client = _httpClientFactory.CreateClient("Api");
            var response = await client.PostAsJsonAsync("api/tournaments/create-tournament", Tournament);

            if (response.IsSuccessStatusCode)
            {
                CreateTournamentSuccess = true;
                CreateTournamentMessage = "Torneo creado correctamente.";
                return RedirectToPage(new { organizerId = OrganizerId });
            }
            else
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                CreateTournamentSuccess = false;
                CreateTournamentMessage = $"Error al crear el torneo: {errorMsg}";
            }

            await OnGetAsync(OrganizerId);
            return Page();
        }
    }
}