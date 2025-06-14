﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFC.AppEventos.Application.DTO;
using TFC.AppEventos.Application.DTO.Responses;

namespace TFC.AppEventos.Application.Interface
{
    public interface IFightersApplication
    {
        Task<FightersDTO> GetFighterById(int user1Id);
        Task<GetFighterInfoResponse> GetFighterInfo(int userId);
        Task<GetUserFighterInfoListResponse> GetFighterList();
        Task<GetMyTournamentsAsFighterResponse> GetMyTournamentsAsFighter(int userId);
        Task<ChangeRoleResponse> RegisterFighter(FightersDTO fighterDto);
        Task<bool> UnregisterFighter(int id);
    }
}
