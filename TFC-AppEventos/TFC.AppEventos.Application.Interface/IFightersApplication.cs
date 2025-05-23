using System;
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
        Task<GetFighterInfoResponse> GetFighterInfo(int userId);
        Task<GetMyTournamentsAsFighterResponse> GetMyTournamentsAsFighter(int userId);
        Task<RegisterFighterResponse> RegisterFighter(FightersDTO fighterDto);
        Task<bool> UnregisterFighter(int id);
    }
}
