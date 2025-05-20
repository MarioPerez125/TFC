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

        public async Task<GetMyTournamentsResponse> GetMyTournaments(int userId)
        {
            GetMyTournamentsResponse response = new GetMyTournamentsResponse();

            Fighter? fighter = _context.Fighters.FirstOrDefault(f => f.UserId == userId);

            IEnumerable<Tournament> tournaments = _context.Tournaments
                .Include(t => t.Participants)
                .Where(t => t.Participants.Any(f => f.UserId == userId))
                .ToList();

            if (tournaments != null && tournaments.Any())
            {
                response.Tournaments = tournaments.Select(t => new TournamentDto
                {
                    TournamentId = t.TournamentId,
                    Name = t.Name,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    OrganizerId = t.OrganizerId,
                    Status = t.Status,
                    SportType = t.SportType,
                    Participants = t.Participants.Select(p => new FighterDto
                    {
                        UserId = p.UserId,
                        Wins = p.Wins,
                        Losses = p.Losses,
                        Draws = p.Draws,
                        WeightClass = p.WeightClass,
                        Height = p.Height,
                        Reach = p.Reach
                    }).ToList()
                }).ToList();

                response.IsSuccess = true;
                response.Message = "Torneos obtenidos correctamente";
                return response;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "No se encontraron torneos para el luchador";
                return response;
            }
        }

        public async Task<RegisterFighterResponse> RegisterFighter(FighterDto fighterDto)
        {
            RegisterFighterResponse response = new RegisterFighterResponse();

            User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == fighterDto.UserId);

            if (user != null)
            {
                user.Role = Roles.Fighter.ToString();
                Fighter fighter = new Fighter
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
            Fighter? fighter = await _context.Fighters.FirstOrDefaultAsync(f => f.UserId == id);
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
