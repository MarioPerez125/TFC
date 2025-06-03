using Microsoft.EntityFrameworkCore;
using TFC.AppEventos.Domain.Entities;

namespace TFC.AppEventos.Database.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Fight> Fights { get; set; }
        public DbSet<Fighter> Fighters { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<FightResult> FightResults { get; set; }
        public DbSet<Participantes> Participantes { get; set; }
    }

}
