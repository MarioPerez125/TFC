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

        public async Task<GetFighterInfoResponse> GetFighterInfo(int userId)
        {
            GetFighterInfoResponse response = new GetFighterInfoResponse();
            try
            {
                User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                Fighter? fighter = _context.Fighters.FirstOrDefault(f => f.UserId == userId);

                if (user != null && fighter != null)
                {
                    response.FighterInfo.User = new UserDto
                    {
                        UserId = user.UserId,
                        Username = user.Username,
                        Email = user.Email,
                        Role = user.Role
                    };
                    response.FighterInfo.Fighter = new FightersDTO
                    {
                        UserId = fighter.UserId,
                        Wins = fighter.Wins,
                        Losses = fighter.Losses,
                        Draws = fighter.Draws,
                        WeightClass = fighter.WeightClass,
                        Height = fighter.Height,
                        Reach = fighter.Reach
                    };
                    response.IsSuccess = true;
                    response.Message = "Información del luchador obtenida correctamente";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Luchador no encontrado";
                }
            }
            catch
            {
                response.IsSuccess = false;
                response.Message = "Error al obtener la información del luchador";
            }
            return response;
        }

        public async Task<GetUserFighterInfoListResponse> GetFighterList()
        {
            GetUserFighterInfoListResponse response = new GetUserFighterInfoListResponse();
            List<Fighter> fighters = await _context.Fighters.ToListAsync();
            foreach (var f in fighters)
            {
                User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == f.UserId);
                if (user != null)
                {
                    FighterForFriendList userFighterInfo = new FighterForFriendList
                    {
                        UserId = user.UserId,
                        Username = user.Username,
                        Email = user.Email,
                        Name = user.Name,
                        LastName = user.LastName,
                        Phone = user.Phone,
                        BirthDate = user.BirthDate,
                        City = user.City,
                        Country = user.Country,
                        Role = user.Role,
                        FighterId = f.FighterId,
                        Wins = f.Wins,
                        Losses = f.Losses,
                        Draws = f.Draws,
                        WeightClass = f.WeightClass,
                        Height = f.Height,
                        Reach = f.Reach
                    };
                    response.fighterList.Add(userFighterInfo);
                }
            }
            response.IsSuccess = true;
            return response;
        }


        public async Task<GetMyTournamentsAsFighterResponse> GetMyTournamentsAsFighter(int userId)
        {
            GetMyTournamentsAsFighterResponse response = new GetMyTournamentsAsFighterResponse();

            try
            {

                Fighter? fighter = _context.Fighters.FirstOrDefault(f => f.UserId == userId);

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

        public async Task<ChangeRoleResponse> RegisterFighter(FightersDTO fighterDto)
        {
            ChangeRoleResponse response = new ChangeRoleResponse();

            User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == fighterDto.UserId);
            user.Role = Roles.Fighter.ToString();


            if (user != null)
            {
                response.User = new UserDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    Name = user.Name,
                    LastName = user.LastName,
                    Phone = user.Phone,
                    BirthDate = user.BirthDate,
                    City = user.City,
                    Country = user.Country,
                    Role = user.Role
                };
                Fighter? existingFighter = await _context.Fighters.FirstOrDefaultAsync(f => f.UserId == user.UserId);
                if (!await _context.Fighters.AnyAsync(f => f.UserId == user.UserId))
                {
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
                }
                else
                {
                    existingFighter.Height = fighterDto.Height;
                    existingFighter.Reach = fighterDto.Reach;
                    existingFighter.WeightClass = fighterDto.WeightClass;
                }
                await _context.SaveChangesAsync();

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
