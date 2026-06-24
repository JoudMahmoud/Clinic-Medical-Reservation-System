using ClinicMedicalReservationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Application.Interfaces
{
    public interface IRoleService
    {
        Task AddUserToRoleAsync(User user, string roleName);
        Task RemoveUserFromAsync(User user, string roleName);
    }
}
