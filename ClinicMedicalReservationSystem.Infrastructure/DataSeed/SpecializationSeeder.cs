using ClinicMedicalReservationSystem.Domain.Entities;
using ClinicMedicalReservationSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Infrastructure.DataSeed
{
    public class SpecializationSeeder
    {
        private readonly ClinicMedicalReservationSystemDbcontext _dbcontext;

        public SpecializationSeeder(ClinicMedicalReservationSystemDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task SeedSpecializationsAsync()
        {
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "DataSeed",
                "Specializations.json");
        

            if (!File.Exists(path))
                return;

            var json = await File.ReadAllTextAsync(path);

            var specializations =
                JsonSerializer.Deserialize<List<Specialization>>(json);//here I show count = 5 but in each one Id = 0 and name = null why?? 

            if (specializations is null)
                return;

            foreach (var specialization in specializations)
            {
                if(!await _dbcontext.Specializations.AnyAsync(s => s.Name == specialization.Name))
                    await _dbcontext.Specializations.AddAsync(specialization);
            }

            await _dbcontext.SaveChangesAsync();
        }
    }
}
