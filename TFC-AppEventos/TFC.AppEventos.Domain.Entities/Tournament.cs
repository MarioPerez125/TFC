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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SportType { get; set; } // "MMA", "Judo", etc.
        public string Status { get; set; } = "Planned"; // "Planned", "Ongoing", "Completed"

        // Clave foránea
        public int OrganizerId { get; set; }
        public User Organizer { get; set; }

        // Relaciones
        public ICollection<Fighter> Participants { get; set; }
        public ICollection<Fight> Fights { get; set; }
    }
}
