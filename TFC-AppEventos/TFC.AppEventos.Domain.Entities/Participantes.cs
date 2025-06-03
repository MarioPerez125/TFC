using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFC.AppEventos.Domain.Entities
{
    public class Participantes
    {
        [Key]
        public int ParticipanteId { get; set; }
        public int UserId { get; set; }
        public int TournamentId { get; set; }
    }
}
