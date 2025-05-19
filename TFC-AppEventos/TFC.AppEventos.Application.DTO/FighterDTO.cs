using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFC.AppEventos.Application.DTO
{
    public class FighterDto
    {
        public int FighterId { get; set; }
        public int UserId { get; set; }
        public int TournamentId { get; set; }
        public string WeightCategory { get; set; }
    }
}
