using Microsoft.EntityFrameworkCore;
using TFC.AppEventos.Application.Interface;
using TFC.AppEventos.Application.Main;
using TFC.AppEventos.Database.Context;
using TFC.AppEventos.Infraestructure.Interface;
using TFC.AppEventos.Infraestructure.Interface.IAuthRepository;
using TFC.AppEventos.Infraestructure.Repository;
using TFC.AppEventos.Infraestructure.Repository.AuthRepository;

namespace OrganizerWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddHttpClient("ApiEventos", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5263/");
            });

            builder.Services.AddScoped<IAuthApplication, AuthApplication>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IFightersApplication, FightersApplication>();
            builder.Services.AddScoped<IFightersRepository, FightersRepository>();
            builder.Services.AddScoped<ITournamentApplication, TournamentApplication>();
            builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
            builder.Services.AddScoped<IFightApplication, FightApplication>();
            builder.Services.AddScoped<IFightRepository, FightRepository>();


            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerConnection")));

            // Configure the HTTP request pipeline.
            var app = builder.Build();
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
