﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFC.AppEventos.Domain.Entities
{
    public class Fighter
    {
        public int FighterId { get; set; }
        public string? WeightClass { get; set; }
        public int? Height { get; set; } = 0; 
        public int? Reach { get; set; } = 0; 
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int Draws { get; set; } = 0;

        public int UserId { get; set; }
        
    }
}
