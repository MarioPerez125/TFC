namespace TFC.AppEventos.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // "Admin", "Organizer", "Fighter"

        // Relaciones
        public ICollection<Tournament> OrganizedTournaments { get; set; }
        public ICollection<Fighter> FighterRegistrations { get; set; }
    }
}
