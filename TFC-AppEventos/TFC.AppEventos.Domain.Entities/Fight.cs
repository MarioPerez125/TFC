using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFC.AppEventos.Domain.Entities
{
    public class Fight
    {
        public int FightId { get; set; }
        public int TournamentId { get; set; }
        public int Fighter1Id { get; set; }
        public int Fighter2Id { get; set; }
        public DateTime? ScheduledTime { get; set; }
        public string? Status { get; set; }
        public int? WinnerId { get; set; }
    }
}
