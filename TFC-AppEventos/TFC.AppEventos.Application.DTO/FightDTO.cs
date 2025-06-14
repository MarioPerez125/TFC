﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFC.AppEventos.Application.DTO
{
    public class FightDto
    {
        public int FightId { get; set; }
        public int TournamentId { get; set; }
        public int Fighter1Id { get; set; }
        public int Fighter2Id { get; set; }
        public string? Status { get; set; }
        public int? WinnerId { get; set; }
        public string? NombrePeleador1 { get; set; }
        public string? NombrePeleador2 { get; set; }
    }
}
