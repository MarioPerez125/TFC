using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
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

        private Dictionary<int, string> userIdToName = new();
        public string ErrorMessage { get; set; }

        // Para el formulario de resultado de pelea
        [BindProperty]
        public FightResultInput FightResult { get; set; }

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

            var fightDto = new FightDto
            {
                TournamentId = TournamentId,
                Fighter1Id = fighter1Id,
                Fighter2Id = fighter2Id
            };

            var result = await _fightApplication.ScheduleFight(fightDto);

            ErrorMessage = result != null && result.IsSuccess ? null : result?.Message ?? "Error al agregar la pelea.";

            // Limpiar selección
            SelectedFighter1Id = 0;
            SelectedFighter2Id = 0;

            // PRG: Redirige para evitar reenvío accidental del formulario
            return RedirectToPage(new { tournamentId = TournamentId, errorMessage = ErrorMessage });
        }

        public async Task<IActionResult> OnPostSetResultAsync()
        {
            if (FightResult == null || FightResult.FightId == 0 || FightResult.WinnerId == null || FightResult.LooserId == null)
            {
                ErrorMessage = "Datos de resultado incompletos.";
                await LoadDataAsync();
                return Page();
            }

            var resultDto = new FightResultDto
            {
                FightId = FightResult.FightId,
                WinnerId = FightResult.WinnerId,
                LooserId = FightResult.LooserId,
                Method = FightResult.Method,
                Duration = FightResult.Duration
            };

            var response = await _fightApplication.SetAWinner(resultDto);

            ErrorMessage = response == null ? "Error al guardar el resultado." : null;

            // PRG: Redirige para evitar reenvío accidental del formulario
            return RedirectToPage(new { tournamentId = TournamentId, errorMessage = ErrorMessage });
        }

        public async Task<IActionResult> OnPostCancelFightAsync(int fightId)
        {
            var result = await _fightApplication.CancelFight(fightId);
            ErrorMessage = result.IsSuccess ? null : result.Message;

            // PRG: Redirige para evitar reenvío accidental del formulario
            return RedirectToPage(new { tournamentId = TournamentId, errorMessage = ErrorMessage });
        }

        private async Task LoadDataAsync()
        {
            TournamentName = $"Torneo #{TournamentId}";

            userIdToName.Clear();

            var participantesResponse = await _tournamentApplication.GetParticipantesParaPelear(TournamentId);

            // Todos los peleadores siempre disponibles
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
                    userIdToName[p.UserId] = displayName;
                }
            }
            FightersSelectList = fightersList;

            // Mapea las peleas para la tabla
            var fightsResponse = await _fightApplication.GetFightsByTournament(TournamentId);
            Fights = fightsResponse?.Fights?
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