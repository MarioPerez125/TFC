using TFC.AppEventos.Domain.Entities.Enum;

namespace TFC.AppEventos.Domain.Entities
{
    public class User
    {
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public int? Phone { get; set; }
        public string? BirthDate { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = Roles.User.ToString();
    }
}

