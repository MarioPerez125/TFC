using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Json;
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

        private Dictionary<int, string> userIdToName = new();
        public string? ErrorMessage { get; set; }

        [BindProperty]
        public FightResultInput FightResult { get; set; }

        private readonly IHttpClientFactory _httpClientFactory;

        public OrganizeFightsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(string errorMessage = null)
        {
            ErrorMessage = errorMessage;
            await LoadDataAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (SelectedFighter1Id == 0 || SelectedFighter2Id == 0)
            {
                ErrorMessage = "Debes seleccionar ambos peleadores.";
                await LoadDataAsync();
                return Page();
            }
            if (SelectedFighter1Id == SelectedFighter2Id)
            {
                ErrorMessage = "No puedes seleccionar el mismo peleador dos veces.";
                await LoadDataAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("Api");
            var fighter1Info = await client.GetFromJsonAsync<FightersDTO>($"api/fighters/user-fighter-info/{SelectedFighter1Id}");
            var fighter2Info = await client.GetFromJsonAsync<FightersDTO>($"api/fighters/user-fighter-info/{SelectedFighter2Id}");

            var fighter1Id = fighter1Info?.FighterId ?? 0;
            var fighter2Id = fighter2Info?.FighterId ?? 0;

            if (fighter1Id == 0 || fighter2Id == 0)
            {
                ErrorMessage = "No se pudo obtener la información de los peleadores.";
                await LoadDataAsync();
                return Page();
            }

            var fightDto = new FightDto
            {
                TournamentId = TournamentId,
                Fighter1Id = fighter1Id,
                Fighter2Id = fighter2Id
            };

            var response = await client.PostAsJsonAsync("api/fights/schedule-fight", fightDto);
            var result = await response.Content.ReadFromJsonAsync<OrganizarPeleaResponse>();

            ErrorMessage = result != null && result.IsSuccess ? null : result?.Message ?? "Error al agregar la pelea.";

            SelectedFighter1Id = 0;
            SelectedFighter2Id = 0;

            return RedirectToPage(new { tournamentId = TournamentId, errorMessage = ErrorMessage });
        }

        public async Task<IActionResult> OnPostSetResultAsync()
        {
            if (FightResult == null || FightResult.FightId == 0 || FightResult.WinnerId == null)
            {
                ErrorMessage = "Datos de resultado incompletos.";
                await LoadDataAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("Api");
            var resultDto = new FightResultDto
            {
                FightId = FightResult.FightId,
                WinnerId = FightResult.WinnerId,
                LooserId = FightResult.LooserId,
                Method = FightResult.Method,
                Duration = FightResult.Duration
            };

            var response = await client.PutAsJsonAsync("api/fights/set-winner", resultDto);
            var result = await response.Content.ReadFromJsonAsync<OrganizarPeleaResponse>();

            ErrorMessage = result == null || !result.IsSuccess ? result?.Message ?? "Error al guardar el resultado." : null;

            return RedirectToPage(new { tournamentId = TournamentId, errorMessage = ErrorMessage });
        }

        public async Task<IActionResult> OnPostCancelFightAsync(int fightId)
        {
            var client = _httpClientFactory.CreateClient("Api");
            var response = await client.DeleteAsync($"api/fights/{fightId}");
            var result = await response.Content.ReadFromJsonAsync<OrganizarPeleaResponse>();
            ErrorMessage = result != null && result.IsSuccess ? null : result?.Message;

            return RedirectToPage(new { tournamentId = TournamentId, errorMessage = ErrorMessage });
        }

        private async Task LoadDataAsync()
        {
            TournamentName = $"Torneo #{TournamentId}";
            userIdToName.Clear();

            var client = _httpClientFactory.CreateClient("Api");
            var participantesList = await client.GetFromJsonAsync<List<ParticipantesDTO>>(
                $"api/tournaments/{TournamentId}/participants-para-pelear");
            var allFighters = await client.GetFromJsonAsync<List<FighterForFriendList>>("api/fighters/user-fighter-list");

            var fightersList = new List<SelectListItem>();
            if (participantesList != null && allFighters != null)
            {
                foreach (var p in participantesList)
                {
                    var fighter = allFighters.FirstOrDefault(f => f.UserId == p.UserId);
                    string displayName = (fighter != null && (!string.IsNullOrWhiteSpace(fighter.Name) || !string.IsNullOrWhiteSpace(fighter.LastName)))
                        ? $"{fighter.Name} {fighter.LastName}".Trim()
                        : $"User {p.UserId}";
                    fightersList.Add(new SelectListItem
                    {
                        Value = p.UserId.ToString(),
                        Text = displayName
                    });
                    userIdToName[p.UserId] = displayName;
                }
            }
            FightersSelectList = fightersList;

            var fightsList = await client.GetFromJsonAsync<List<FightDto>>($"api/fights/tournament/{TournamentId}");
            Fights = fightsList?
                .Select(f =>
                {
                    string winnerName = null;
                    if (f.WinnerId.HasValue && f.Status != null && f.Status.ToUpper() != "ONGOING")
                    {
                        if (f.WinnerId == f.Fighter1Id)
                            winnerName = !string.IsNullOrWhiteSpace(f.NombrePeleador1) ? f.NombrePeleador1 : f.Fighter1Id.ToString();
                        else if (f.WinnerId == f.Fighter2Id)
                            winnerName = !string.IsNullOrWhiteSpace(f.NombrePeleador2) ? f.NombrePeleador2 : f.Fighter2Id.ToString();
                        else if (userIdToName.TryGetValue(f.WinnerId.Value, out var name))
                            winnerName = name;
                        else
                            winnerName = f.WinnerId.Value.ToString();
                    }

                    return new FightViewModel
                    {
                        FightId = f.FightId,
                        Fighter1Id = f.Fighter1Id,
                        Fighter2Id = f.Fighter2Id,
                        Fighter1Name = !string.IsNullOrWhiteSpace(f.NombrePeleador1) ? f.NombrePeleador1 : f.Fighter1Id.ToString(),
                        Fighter2Name = !string.IsNullOrWhiteSpace(f.NombrePeleador2) ? f.NombrePeleador2 : f.Fighter2Id.ToString(),
                        Status = f.Status,
                        WinnerName = winnerName
                    };
                })
                .ToList() ?? new List<FightViewModel>();
        }

        public class FightViewModel
        {
            public int FightId { get; set; }
            public int Fighter1Id { get; set; }
            public int Fighter2Id { get; set; }
            public string Fighter1Name { get; set; }
            public string Fighter2Name { get; set; }
            public string Status { get; set; }
            public string WinnerName { get; set; }
        }

        public class FightResultInput
        {
            public int FightId { get; set; }
            public int? WinnerId { get; set; }
            public int? LooserId { get; set; }
            public string Method { get; set; }
            public TimeSpan? Duration { get; set; }
        }
    }
}