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
    public class FightRepository : IFightRepository
    {
        private readonly AppDbContext _context;
        public FightRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrganizarPeleaResponse> CancelFight(int id)
        {
            OrganizarPeleaResponse organizarPeleaResponse = new OrganizarPeleaResponse();
            try
            {
                var fight = await _context.Fights.FindAsync(id);
                if (fight == null)
                {
                    organizarPeleaResponse.IsSuccess = false;
                    organizarPeleaResponse.Message = "Pelea no encontrada";
                    return organizarPeleaResponse;
                }

                // Solo permite borrar si NO está finalizada
                if (string.Equals(fight.Status, "Completed", StringComparison.OrdinalIgnoreCase))
                {
                    organizarPeleaResponse.IsSuccess = false;
                    organizarPeleaResponse.Message = "No se puede cancelar una pelea finalizada.";
                    return organizarPeleaResponse;
                }

                _context.Fights.Remove(fight);
                await _context.SaveChangesAsync();

                organizarPeleaResponse.IsSuccess = true;
                organizarPeleaResponse.Message = "Pelea cancelada exitosamente";
            }
            catch (Exception ex)
            {
                organizarPeleaResponse.IsSuccess = false;
                organizarPeleaResponse.Message = $"Error al cancelar la pelea: {ex.Message}";
            }
            return organizarPeleaResponse;
        }

        public async Task<GetFightsByTournamentResponse> GetFightsByTournament(int tournamentId)
        {
            GetFightsByTournamentResponse response = new GetFightsByTournamentResponse();
            try
            {
                List<Fight> fights = await _context.Fights
                    .Where(f => f.TournamentId == tournamentId).ToListAsync();

                if (fights == null || fights.Count == 0)
                {
                    response.Fights = new List<FightDto>();
                    response.IsSuccess = true;
                    response.Message = "No hay peleas registradas para este torneo.";
                    return response;
                }
                else
                {
                    fights.ForEach(f =>
                    {
                        Fighter? fighter = _context.Fighters.FirstOrDefault(fighter => fighter.FighterId == f.Fighter1Id);
                        Fighter? fighter2 = _context.Fighters.FirstOrDefault(fighter => fighter.FighterId == f.Fighter2Id);

                        User? user1 = _context.Users.FirstOrDefault(u => u.UserId == fighter.UserId);
                        User? user2 = _context.Users.FirstOrDefault(u => u.UserId == fighter2.UserId);

                        response.Fights.Add(new FightDto
                        {
                            FightId = f.FightId,
                            Fighter1Id = f.Fighter1Id,
                            Fighter2Id = f.Fighter2Id,
                            TournamentId = f.TournamentId,
                            Status = f.Status,
                            WinnerId = f.WinnerId,
                            NombrePeleador1 = user1.Name + " " + user1.LastName,
                            NombrePeleador2 = user2.Name + " " + user2.LastName
                        });
                    });

                    response.IsSuccess = true;
                    response.Message = "Fights retrieved successfully.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Error retrieving fights: " + ex.Message;
            }
            return response;
        }

        public async Task<GetMyFightsResponse> GetMyFights(int userId)
        {
            GetMyFightsResponse response = new GetMyFightsResponse();
            try
            {
                Fighter? fighter = await _context.Fighters.FirstOrDefaultAsync(f => f.UserId == userId);
                if (fighter == null)
                {
                    throw new Exception("Fighter not found.");
                }
                List<Fight> fights = await _context.Fights
                    .Where(f => f.Fighter1Id == fighter.FighterId || f.Fighter2Id == fighter.FighterId)
                    .ToListAsync();

                fights.ForEach(f =>
                {
                    response.Fights.Add(new FightDto
                    {
                        FightId = f.FightId,
                        Fighter1Id = f.Fighter1Id,
                        Fighter2Id = f.Fighter2Id,
                        TournamentId = f.TournamentId,
                        Status = f.Status,
                        WinnerId = f.WinnerId
                    });
                });
                response.IsSuccess = true;
                response.Message = "Fights retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Error retrieving fights: " + ex.Message;
            }
            return response;
        }

        public async Task<OrganizarPeleaResponse> ScheduleFight(FightDto fightDto)
        {
            OrganizarPeleaResponse response = new OrganizarPeleaResponse();

            try
            {
                Fighter? fighter1 = await _context.Fighters.FirstOrDefaultAsync(f => f.FighterId == fightDto.Fighter1Id);
                Fighter? fighter2 = await _context.Fighters.FirstOrDefaultAsync(f => f.FighterId == fightDto.Fighter2Id);

                if (fighter1 == null || fighter2 == null)
                {
                    response.IsSuccess = false;
                    response.Message = "No se encontró uno o ambos peleadores para crear la pelea.";
                    return response;
                }

                Fight fight = new Fight
                {
                    Fighter1Id = fightDto.Fighter1Id,
                    Fighter2Id = fightDto.Fighter2Id,
                    TournamentId = fightDto.TournamentId,
                    Status = "ONGOING"
                };

                await _context.Fights.AddAsync(fight);
                await _context.SaveChangesAsync();
                response.IsSuccess = true;
                response.Message = "Fight scheduled successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Error scheduling fight: " + ex.Message;
            }
            return response;
        }

        public async Task<object?> SetAWinner(FightResultDto resultDto)
        {
            Fight? fight = await _context.Fights.FirstOrDefaultAsync(f => f.FightId == resultDto.FightId);
            fight.Status = "FINISHED";
            if (fight == null)
            {
                return new { IsSuccess = false, Message = "Fight not found." };
            }
            else
            {
                fight.WinnerId = resultDto.WinnerId;
                fight.Status = "Completed";
                await _context.SaveChangesAsync();

                FightResult fightResult = new FightResult
                {
                    FightId = resultDto.FightId,
                    WinnerId = resultDto.WinnerId,
                    Duration = resultDto.Duration,
                    Method = resultDto.Method

                };
                Fighter? fighter = await _context.Fighters.FirstOrDefaultAsync(f => f.FighterId == resultDto.WinnerId);
                fighter.Wins += 1;
                await _context.SaveChangesAsync();

                Fighter? loserFighter = await _context.Fighters.FirstOrDefaultAsync(f => f.FighterId == resultDto.LooserId);
                loserFighter.Losses += 1;
                await _context.SaveChangesAsync();


                await _context.FightResults.AddAsync(fightResult);
                await _context.SaveChangesAsync();
                return new { IsSuccess = true, Message = "Winner set successfully." };
            }
        }
    }
}
