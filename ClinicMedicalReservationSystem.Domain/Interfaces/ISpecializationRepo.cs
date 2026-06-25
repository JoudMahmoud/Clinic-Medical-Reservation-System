using ClinicMedicalReservationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Domain.Interfaces
{
    public interface ISpecializationRepo
    {
        Task AddAsync(Specialization specialization);
        Task<bool> SaveChangesAsync();

    }
}
