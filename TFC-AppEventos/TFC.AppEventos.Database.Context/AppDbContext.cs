using Microsoft.EntityFrameworkCore;
using TFC.AppEventos.Domain.Entities;

namespace TFC.AppEventos.Database.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Fight> Fights { get; set; }
        public DbSet<Fighters> Fighters { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }

    }
}
