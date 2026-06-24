using ClinicMedicalReservationSystem.Domain.Entities;
using ClinicMedicalReservationSystem.Domain.Interfaces;
using ClinicMedicalReservationSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Infrastructure.Repository
{
    public class RefreshTokenRepo:IRefreshTokenRepo
    {
        #region Fields
        private readonly ClinicMedicalReservationSystemDbcontext _dbcontext;
        #endregion

        #region Constructor
        public RefreshTokenRepo(ClinicMedicalReservationSystemDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        #endregion
        
        #region CRUD Operations
        public async Task AddAsync(RefreshToken refreshToken)
        {
            await _dbcontext.RefreshTokens.AddAsync(refreshToken);
        }
        public async Task<RefreshToken?> GetByTokenAsync(string refreshToken)
        {
            return await _dbcontext.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        }
        public async Task<RefreshToken?> GetByTokenandUserIdAsync(string userId, string token)
        {
            return await _dbcontext.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token && rt.UserId == userId);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return await _dbcontext.SaveChangesAsync() > 0;
        }
        #endregion
    }
}
