using AutoMapper;
using ClinicMedicalReservationSystem.Application.Common;
using ClinicMedicalReservationSystem.Application.DTOs;
using ClinicMedicalReservationSystem.Application.Interfaces;
using ClinicMedicalReservationSystem.Domain.Entities;
using ClinicMedicalReservationSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Application.Services
{
    public class SpecializationService:ISpecializationService
    {
        #region Fields
        private readonly ISpecializationRepo _specializationRepo;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor
        public SpecializationService(ISpecializationRepo specializationRepo, IMapper mapper)
        {
            _specializationRepo = specializationRepo;
            _mapper = mapper;
        }
        #endregion

        #region CRUD Operations 
        public async Task<Result> AddAsync(SpecializationDto dto)
        {
            var specialization =  _mapper.Map<Specialization>(dto);
            await _specializationRepo.AddAsync(specialization);

            var saved = await _specializationRepo.SaveChangesAsync();
            if (!saved)
                return Result.Failure("Failure to create Specializtion.");

            return Result.Success("Specialization Created.");
        }
        #endregion
    }
}
