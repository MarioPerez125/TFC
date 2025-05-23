using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFC.AppEventos.Application.DTO
{
    public class UserDto
    {
        public int? UserId { get; set; }
        public int Name { get; set; }
        public int LastName { get; set; }
        public int Phone { get; set; }
        public DateOnly? BirthDate { get; set; }
        public int? City { get; set; }
        public int? Country { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Role { get; set; }
    }
}
