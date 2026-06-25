using ClinicMedicalReservationSystem.Application.Common;
using ClinicMedicalReservationSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Application.Interfaces
{
    public interface ISpecializationService
    {
        Task<Result> AddAsync(SpecializationDto dto);
    }
}
