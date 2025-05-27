using System.ComponentModel.DataAnnotations;

namespace TFC.AppEventos.Application.DTO
{
    public class RegisterDTO
    {

        public string Name { get; set; }
        public string LastName { get; set; }
        public int? Phone { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Role { get; set; }
    }
}
