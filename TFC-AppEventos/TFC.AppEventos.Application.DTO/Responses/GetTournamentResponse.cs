using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFC.AppEventos.Transversal.Utils;

namespace TFC.AppEventos.Application.DTO.Responses
{
    public class GetTournamentResponse : BaseResponse
    {
        public List<TournamentDto>? Tournament { get; set; } = new List<TournamentDto>();
    }
}
