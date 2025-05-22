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
using TFC.AppEventos.Domain.Entities.Enum;
using TFC.AppEventos.Infraestructure.Interface;

namespace TFC.AppEventos.Infraestructure.Repository
{
    public class FightersRepository : IFightersRepository
    {
        private readonly AppDbContext _context;

        public FightersRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GetMyTournamentsAsFighterResponse> GetMyTournamentsAsFighter(int userId)
        {
            GetMyTournamentsAsFighterResponse response = new GetMyTournamentsAsFighterResponse();

            try
            {

                Fighters? fighter = _context.Fighters.FirstOrDefault(f => f.UserId == userId);

                var tournamentIds = await _context.Fights
    .Where(f => f.Fighter1Id == fighter.FighterId || f.Fighter2Id == fighter.FighterId)
    .Select(f => f.TournamentId)
    .Distinct()
    .ToListAsync();

                IEnumerable<TournamentDto> tournaments = await _context.Tournaments
                    .Where(t => tournamentIds.Contains(t.TournamentId))
                    .Select(t => new TournamentDto
                    {
                        TournamentId = t.TournamentId,
                        location = t.Name,
                        StartDate = t.StartDate,
                        EndDate = t.EndDate,
                        OrganizerId = t.OrganizerId,
                        SportType = t.SportType
                    })
                    .ToListAsync();


                response.Tournaments = tournaments;
                response.IsSuccess = true;
                response.Message = "Torneos obtenidos correctamente";
                return response;
            }
            catch
            {
                response.IsSuccess = false;
                response.Message = "No se encontraron torneos para el luchador";
                return response;
            }
        }
        
        public async Task<RegisterFighterResponse> RegisterFighter(FightersDTO fighterDto)
        {
            RegisterFighterResponse response = new RegisterFighterResponse();

            User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == fighterDto.UserId);

            if (user != null)
            {
                user.Role = Roles.Fighter.ToString();
                Fighters fighter = new Fighters
                {
                    UserId = fighterDto.UserId,
                    Wins = fighterDto.Wins,
                    Losses = fighterDto.Losses,
                    Draws = fighterDto.Draws,
                    WeightClass = fighterDto.WeightClass,
                    Height = fighterDto.Height,
                    Reach = fighterDto.Reach
                };
                await _context.Fighters.AddAsync(fighter);
                Console.WriteLine(fighter.ToString());
                await _context.SaveChangesAsync();

                response.Fighter = fighterDto;
                response.IsSuccess = true;
                response.Message = "Luchador registrado correctamente";
                return response;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Usuario no encontrado";
                return response;
            }
        }

        public async Task<bool> UnregisterFighter(int id)
        {
            Fighters? fighter = await _context.Fighters.FirstOrDefaultAsync(f => f.UserId == id);
            if (fighter != null)
            {
                _context.Fighters.Remove(fighter);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
