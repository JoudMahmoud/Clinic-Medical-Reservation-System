using ClinicMedicalReservationSystem.API.ExceptionHandling;
using ClinicMedicalReservationSystem.API.Extensions;
using ClinicMedicalReservationSystem.Application.Automapper;
using ClinicMedicalReservationSystem.Application.Interfaces;
using ClinicMedicalReservationSystem.Application.Services;
using ClinicMedicalReservationSystem.Domain.Entities;
using ClinicMedicalReservationSystem.Domain.Interfaces;
using ClinicMedicalReservationSystem.Infrastructure.DataSeed;
using ClinicMedicalReservationSystem.Infrastructure.Persistence;
using ClinicMedicalReservationSystem.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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


        //swagger / openAPI
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // CORS - allow Authorization header and JSON from any origin (adjust for production)
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        //configure SQL Server DbContext
        builder.Services.AddDbContext<ClinicMedicalReservationSystemDbcontext>(
            options => options.UseLazyLoadingProxies()
            .UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")));


        //register Identity
        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ClinicMedicalReservationSystemDbcontext>()
            .AddDefaultTokenProviders();


        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                   ValidAudience = builder.Configuration["JWT:ValidAudience"],
                   IssuerSigningKey = new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
               };

               options.Events = new JwtBearerEvents
               {
                   OnAuthenticationFailed = context =>
                   {
                       Console.WriteLine($"AUTH FAILED: {context.Exception.Message}");
                       return Task.CompletedTask;
                   }
               };
           });

        //add autoMapper
        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });


        //Rate liming
        builder.Services.AddCustomRateLimiting();

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        //Register Data seeders
        builder.Services.AddScoped<RoleSeeder>();


        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        builder.Services.AddScoped<IRefreshTokenRepo, RefreshTokenRepo>();
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
        app.UseRateLimiter();

        app.UseExceptionHandler();
        // Use CORS before authentication/authorization
        app.UseCors("AllowAll");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        #endregion

        await app.RunAsync();
    }
}