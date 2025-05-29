using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFC.AppEventos.Application.DTO
{
    public class FighterForFriendList
    {
        public int? UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int? Phone { get; set; }
        public string BirthDate { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Role { get; set; } 
        public int FighterId { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public string WeightClass { get; set; }
        public int? Height { get; set; }
        public int? Reach { get; set; } 
    }
}
