using TFC.AppEventos.Domain.Entities.Enum;

namespace TFC.AppEventos.Domain.Entities
{
    public class User
    {
        public int? UserId { get; set; }
        public int Name { get; set; }
        public int LastName { get; set; }
        public int? Phone { get; set; }
        public DateOnly? BirthDate { get; set; }
        public int? City { get; set; }
        public int? Country { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Role { get; set; } = Roles.User.ToString();

        // Relaciones
    }
}

