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

        public async Task<GetFightsByTournamentResponse> GetFightsByTournament(int tournamentId)
        {
            GetFightsByTournamentResponse response = new GetFightsByTournamentResponse();
            try
            {

                List<Fight> fights = await _context.Fights
                    .Where(f => f.TournamentId == tournamentId).ToListAsync();

                if (fights == null || fights.Count == 0)
                {
                    throw new Exception("No fights found for the specified tournament.");
                }
                else
                {
                    fights.ForEach(f =>
                    {
                        Fighter fighter = _context.Fighters.FirstOrDefault(fighter => fighter.FighterId == f.Fighter1Id);
                        Fighter fighter2 = _context.Fighters.FirstOrDefault(fighter => fighter.FighterId == f.Fighter2Id);

                        User user1 = _context.Users.FirstOrDefault(u => u.UserId == fighter.UserId);
                        User user2 = _context.Users.FirstOrDefault(u => u.UserId == fighter2.UserId);

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

                Fight fight = new Fight
                {
                    Fighter1Id = fightDto.Fighter1Id,
                    Fighter2Id = fightDto.Fighter2Id,
                    TournamentId = fightDto.TournamentId,
                };
                Fighter? fighter1 = await _context.Fighters.FirstOrDefaultAsync(f => f.FighterId == fightDto.Fighter1Id);
                Fighter? fighter2 = await _context.Fighters.FirstOrDefaultAsync(f => f.FighterId == fightDto.Fighter2Id);

                if (fighter1 == null || fighter2 == null)
                {
                    User? user1 = await _context.Users.FirstOrDefaultAsync(u => u.UserId == fighter1.UserId);
                    User? user2 = await _context.Users.FirstOrDefaultAsync(u => u.UserId == fighter2.UserId);
                    response.NombrePeleador1 = user1.Name + " " + user1.LastName;
                    response.NombrePeleador2 = user2.Name + " " + user2.LastName;
                }
                
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
