using ClinicMedicalReservationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Domain.Interfaces
{
    public interface IRefreshTokenRepo
    {
        Task AddAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetByTokenAsync(string refreshToken);
        Task<RefreshToken?> GetByTokenandUserIdAsync(string userId, string token);
        Task<bool> SaveChangesAsync();
    }
}
