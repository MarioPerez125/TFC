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

        public Task<OrganizarPeleaResponse> ScheduleFight(FightDto fightDto1)
        {
            Fight fight = new Fight
            {
                Fighter1Id = fightDto1.Fighter1Id,
                Fighter2Id = fightDto2.Fighter2Id,
                TournamentId = fightDto1.TournamentId,
             };
        }
    }
}
