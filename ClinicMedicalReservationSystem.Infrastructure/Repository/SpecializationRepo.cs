using ClinicMedicalReservationSystem.Domain.Entities;
using ClinicMedicalReservationSystem.Domain.Interfaces;
using ClinicMedicalReservationSystem.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Infrastructure.Repository
{
    public class SpecializationRepo:ISpecializationRepo
    {
        #region Fields
        private readonly ClinicMedicalReservationSystemDbcontext _dbcontext;
        #endregion

        #region Constructor
        public SpecializationRepo(ClinicMedicalReservationSystemDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        #endregion

        #region CURD Operations 
        public async Task AddAsync(Specialization specialization)
        {
            await _dbcontext.Specializations.AddAsync(specialization);
        }
        public async Task<Specialization?> GetByIdAsync(int id)
        {
            return await _dbcontext.Specializations.FindAsync(id);
        }

        public void Update(Specialization specialization)
        {
            _dbcontext.Specializations.Update(specialization);
        }
        public void Delete(Specialization specialization)
        {
            _dbcontext.Specializations.Remove(specialization);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _dbcontext.SaveChangesAsync() > 0;
        }
        #endregion

    }
}
