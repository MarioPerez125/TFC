using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;
using TFC.AppEventos.Database.Context;
using TFC.AppEventos.Domain.Entities;
using TFC.AppEventos.Infraestructure.Interface;

namespace TFC.AppEventos.Infraestructure.Repository
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly AppDbContext _context;

        public TournamentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CreateTournamentResponse> CreateTournament(TournamentDto tournamentDto, int organizerId)
        {
            CreateTournamentResponse response = new CreateTournamentResponse();
            try
            {

                Tournament tournament = new Tournament();
                tournament.Name = tournamentDto.location;
                tournament.StartDate = tournamentDto.StartDate;
                tournament.Arena = tournamentDto.Arena; // Assuming Arena is part of the TournamentDto
                tournament.EndDate = tournamentDto.EndDate;
                tournament.SportType = tournamentDto.SportType;
                tournament.OrganizerId = organizerId;

                await _context.Tournaments.AddAsync(tournament);
                await _context.SaveChangesAsync();

                response.IsSuccess = true;
                response.Message = "Torneo creado correctamente";
                response.Tournament = new TournamentDto
                {
                    TournamentId = tournament.TournamentId,
                    location = tournament.Name,
                    Arena = tournament.Arena, // Assuming Arena is part of the TournamentDto
                    StartDate = tournament.StartDate,
                    EndDate = tournament.EndDate,
                    SportType = tournament.SportType,
                    OrganizerId = tournament.OrganizerId
                };

                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error al crear el torneo: {ex.Message}";
                return response;
            }
        }

        public async Task<List<TournamentDto>?> GetAllTournaments()
        {
            List<TournamentDto>? tournaments = new List<TournamentDto>();
            tournaments = await _context.Tournaments
                .Select(t => new TournamentDto
                {
                    TournamentId = t.TournamentId,
                    location = t.Name,
                    Arena = t.Arena,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    SportType = t.SportType,
                    OrganizerId = t.OrganizerId
                })
                .ToListAsync();
            return tournaments;
        }

        public async Task<GetAllParticipantsResponse> GetParticipants(int tournamentId)
        {
            var response = new GetAllParticipantsResponse();
            try
            {
                // Incluye las peleas del torneo y los luchadores de cada pelea
                var fighter1Query = from fight in _context.Fights
                                    where fight.TournamentId == tournamentId
                                    select fight.Fighter1Id;

                var fighter2Query = from fight in _context.Fights
                                    where fight.TournamentId == tournamentId
                                    select fight.Fighter2Id;

                var allFighterIds = fighter1Query
                    .Union(fighter2Query);

                var fighters = await (
                    from f in _context.Fighters
                    join id in allFighterIds on f.FighterId equals id
                    select f
                ).ToListAsync();

                response.Participants = fighters
                    .Select(f => new FightersDTO
                    {
                        UserId = f.UserId,
                        WeightClass = f.WeightClass,
                        Height = f.Height,
                        Reach = f.Reach,
                        Wins = f.Wins,
                        Losses = f.Losses,
                        Draws = f.Draws
                    })
                    .ToList();

                response.IsSuccess = true;
                response.Message = "Participantes obtenidos correctamente";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error al obtener los participantes: {ex.Message}";
            }
            return response;
        }


        public async Task<GetTournamentResponse> GetTournamentByName(string name)
        {
            GetTournamentResponse response = new GetTournamentResponse
            {
                Tournament = new List<TournamentDto>() // Initialize the list to avoid null reference
            };

            IEnumerable<Tournament>? tournament = await _context.Tournaments.Where(x => x.Name == name).ToListAsync();

            if (tournament != null && tournament.Any()) // Check if the collection is not null and has elements
            {
                foreach (var item in tournament)
                {
                    response.Tournament.Add(new TournamentDto
                    {
                        TournamentId = item.TournamentId,
                        location = item.Name,
                        Arena = item.Arena, // Assuming Arena is part of the TournamentDto
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        SportType = item.SportType,
                        OrganizerId = item.OrganizerId
                    });
                }

                response.IsSuccess = true;
                response.Message = "Torneo encontrado correctamente";
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "No se encontró el torneo";
            }

            return response;
        }
    }
}
