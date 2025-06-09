using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFC.AppEventos.Domain.Entities
{
    public class FightResult
    {
        public int FightResultId { get; set; }
        public int FightId { get; set; }
        public int? WinnerId { get; set; }
        public string Method { get; set; } 
        public TimeSpan? Duration { get; set; } 
    }
}
