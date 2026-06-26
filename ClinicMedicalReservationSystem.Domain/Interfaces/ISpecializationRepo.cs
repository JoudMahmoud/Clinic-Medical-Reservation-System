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
        Task<IEnumerable<Specialization>> GetAllAsync(string? search);
        Task AddAsync(Specialization specialization);
        Task<Specialization?> GetByIdAsync(int id);
        Task<Specialization?> GetByIdWithDoctorsAsync(int id);
        Task<Specialization?> GetByNameAsync(string name);
        void Update(Specialization specialization);
        void Delete(Specialization specialization);
        Task<bool> SaveChangesAsync();

    }
}
