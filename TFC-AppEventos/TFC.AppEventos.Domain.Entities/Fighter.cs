using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFC.AppEventos.Domain.Entities
{
    public class Fighter
    {
        public int FighterId { get; set; }
        public string WeightCategory { get; set; }

        // Claves foráneas
        public int UserId { get; set; }
        public User User { get; set; }

        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }

        // Relaciones
        public ICollection<Fight> FightsAsFighter1 { get; set; }
        public ICollection<Fight> FightsAsFighter2 { get; set; }
        public ICollection<Fight> Wins { get; set; }
    }
}
