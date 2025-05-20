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
        public string WeightClass { get; set; }
        public int Height { get; set; } = 0; // Default value
        public int Reach { get; set; } = 0; // Default value
        public int Wins { get; set; } = 0; // Default value
        public int Losses { get; set; } = 0;
        public int Draws { get; set; } = 0;

        // Claves foráneas
        public int UserId { get; set; }
        public User User { get; set; }
        
    }
}
