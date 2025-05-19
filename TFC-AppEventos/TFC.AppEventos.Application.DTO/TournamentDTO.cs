using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFC.AppEventos.Application.DTO
{
    public class TournamentDto
    {
        public int TournamentId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SportType { get; set; }
        public string Status { get; set; }
        public int OrganizerId { get; set; }
    }
}
