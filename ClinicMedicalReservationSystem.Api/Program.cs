
using ClinicMedicalReservationSystem.Domain.Entities;
using ClinicMedicalReservationSystem.Infrastructure.DataSeed;
using ClinicMedicalReservationSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Add Services
        // Add services to the container.
        builder.Services.AddControllers();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        
        //swagger / openAPI
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        //configure SQL Server DbContext
        builder.Services.AddDbContext<ClinicMedicalReservationSystemDbcontext>(
            options => options.UseLazyLoadingProxies()
            .UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")));

        //register Identity
        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ClinicMedicalReservationSystemDbcontext>()
            .AddDefaultTokenProviders();

        //Register Data seeders
        builder.Services.AddScoped<RoleSeeder>();

        #endregion

        var app = builder.Build();

        #region Seed Initial Data
        //create default roles when the application starts

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var roleSeeder = services.GetRequiredService<RoleSeeder>();
                await roleSeeder.seedRolesAsync();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding roles.");
            }
        }

        #endregion
        #region Middleware Pipeline
        //openAPI endpoint
        app.MapOpenApi();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {   
            app.UseSwagger();
            app.UseSwaggerUI();
        }
       

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        #endregion

        await app.RunAsync();
    }
}
