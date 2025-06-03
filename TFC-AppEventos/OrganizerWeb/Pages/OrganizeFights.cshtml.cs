using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
// ...otros using necesarios...
using TFC.AppEventos.Application.Interface;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;

namespace OrganizerWeb.Pages
{
    public class OrganizeFightsModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int TournamentId { get; set; }

        public string TournamentName { get; set; }

        public List<SelectListItem> FightersSelectList { get; set; }

        [BindProperty]
        public int SelectedFighter1Id { get; set; }

        [BindProperty]
        public int SelectedFighter2Id { get; set; }

        public List<FightViewModel> Fights { get; set; }

        public string ErrorMessage { get; set; }

        private readonly ITournamentApplication _tournamentApplication;
        private readonly IFightApplication _fightApplication;
        private readonly IFightersApplication _fightersApplication;

        public OrganizeFightsModel(
            ITournamentApplication tournamentApplication,
            IFightApplication fightApplication,
            IFightersApplication fightersApplication)
        {
            _tournamentApplication = tournamentApplication;
            _fightApplication = fightApplication;
            _fightersApplication = fightersApplication;
        }

        public async Task OnGetAsync()
        {
            await LoadDataAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (SelectedFighter1Id == SelectedFighter2Id)
            {
                ErrorMessage = "No puedes seleccionar el mismo peleador dos veces.";
                await LoadDataAsync();
                return Page();
            }

            // Obtener FighterId de cada UserId seleccionado
            var fighter1Info = await _fightersApplication.GetFighterInfo(SelectedFighter1Id);
            var fighter2Info = await _fightersApplication.GetFighterInfo(SelectedFighter2Id);

            var fighter1Id = fighter1Info?.FighterInfo?.Fighter?.FighterId ?? 0;
            var fighter2Id = fighter2Info?.FighterInfo?.Fighter?.FighterId ?? 0;

            if (fighter1Id == 0 || fighter2Id == 0)
            {
                ErrorMessage = "No se pudo obtener la información de los peleadores.";
                await LoadDataAsync();
                return Page();
            }

            // Agregar pelea usando los FighterId
            var fightDto = new FightDto
            {
                TournamentId = TournamentId,
                Fighter1Id = fighter1Id,
                Fighter2Id = fighter2Id
                // Agrega otros campos si tu DTO lo requiere
            };

            var result = await _fightApplication.ScheduleFight(fightDto);

            if (result != null && result.IsSuccess)
            {
                ErrorMessage = null;
            }
            else
            {
                ErrorMessage = result?.Message ?? "Error al agregar la pelea.";
            }

            await LoadDataAsync();
            return Page();
        }

        private async Task LoadDataAsync()
        {
            TournamentName = $"Torneo #{TournamentId}";

            // Obtener participantes del torneo
            var participantesResponse = await _tournamentApplication.GetParticipantesParaPelear(TournamentId);

            var fightersList = new List<SelectListItem>();
            if (participantesResponse?.Participantes != null)
            {
                foreach (var p in participantesResponse.Participantes)
                {
                    var fighterInfo = await _fightersApplication.GetFighterInfo(p.UserId);
                    var user = fighterInfo?.FighterInfo?.User;
                    string displayName = (user != null && (!string.IsNullOrWhiteSpace(user.Name) || !string.IsNullOrWhiteSpace(user.LastName)))
                        ? $"{user.Name} {user.LastName}".Trim()
                        : $"User {p.UserId}";
                    fightersList.Add(new SelectListItem
                    {
                        Value = p.UserId.ToString(),
                        Text = displayName
                    });
                }
            }
            FightersSelectList = fightersList;

            // Obtener peleas existentes usando los nombres completos del DTO
            var fightsResponse = await _fightApplication.GetFightsByTournament(TournamentId);
            Fights = fightsResponse?.Fights?
                .Select(f => new FightViewModel
                {
                    Fighter1Name = !string.IsNullOrWhiteSpace(f.NombrePeleador1) ? f.NombrePeleador1 : f.Fighter1Id.ToString(),
                    Fighter2Name = !string.IsNullOrWhiteSpace(f.NombrePeleador2) ? f.NombrePeleador2 : f.Fighter2Id.ToString(),
                    Status = f.Status
                })
                .ToList() ?? new List<FightViewModel>();
        }

        public class FightViewModel
        {
            public string Fighter1Name { get; set; }
            public string Fighter2Name { get; set; }
            public string Status { get; set; }
        }
    }
}
