using ClinicMedicalReservationSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Infrastructure.Persistence
{
    public class ClinicMedicalReservationSystemDbcontext:IdentityDbContext<User>
    {
        public ClinicMedicalReservationSystemDbcontext(DbContextOptions<ClinicMedicalReservationSystemDbcontext> options):base(options) { }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleChangeRequest> ScheduleChanges { get; set; }
        public DbSet<Specialization> Specializations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
                
            base.OnModelCreating(builder);
            builder.Entity<User>().HasIndex(u => u.NormalizedUserName)
                .IsUnique(false);
            builder.Entity<User>()
             .HasIndex(u => u.NormalizedEmail)
             .IsUnique();
            builder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();
            builder.Entity<User>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();
        }

    }
}
