﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;

namespace TFC.AppEventos.Infraestructure.Interface
{
    public interface IFightRepository
    {
        Task<OrganizarPeleaResponse> CancelFight(int id);
        Task<GetFightsByTournamentResponse> GetFightsByTournament(int tournamentId);
        Task<GetMyFightsResponse> GetMyFights(int userId);
        Task<OrganizarPeleaResponse> ScheduleFight(FightDto fightDto);
        Task<object?> SetAWinner(FightResultDto resultDto);
    }
}
