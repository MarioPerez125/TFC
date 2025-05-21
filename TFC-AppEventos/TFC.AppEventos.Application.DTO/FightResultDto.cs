using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFC.AppEventos.Application.DTO
{
    public class FightResultDto
    {
        public int FightId { get; set; }
        public int? WinnerId { get; set; }
        public string Method { get; set; } // KO, TKO, Submission, Decision, etc.
        public TimeSpan? Duration { get; set; } // Duration of the fight
    }
}
