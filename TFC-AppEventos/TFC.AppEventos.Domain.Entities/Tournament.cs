using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFC.AppEventos.Domain.Entities
{
    public class Tournament
    {
        public int TournamentId { get; set; }
        public string Name { get; set; }
        public string Arena { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string SportType { get; set; } // "MMA", "Judo", etc.
        // Clave foránea
        public int OrganizerId { get; set; }

    }
}
