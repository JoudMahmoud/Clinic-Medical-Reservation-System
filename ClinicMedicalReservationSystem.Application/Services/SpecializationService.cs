using AutoMapper;
using ClinicMedicalReservationSystem.Application.Common;
using ClinicMedicalReservationSystem.Application.DTOs;
using ClinicMedicalReservationSystem.Application.Interfaces;
using ClinicMedicalReservationSystem.Domain.Entities;
using ClinicMedicalReservationSystem.Domain.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
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

        public async Task<SpecializationReviewDto?> GetByIdAsync(int id)
        {
            var exsitSpecialization = await _specializationRepo.GetByIdAsync(id);
           
            if(exsitSpecialization == null)
                return null;
            
            var specializationDto = _mapper.Map<SpecializationReviewDto>(exsitSpecialization);
            return specializationDto;
        }

        public async Task<SpecializationReviewDto?> UpdateAsync(int id, SpecializationDto dto)
        {
            var existSpecialization = await _specializationRepo.GetByIdAsync(id);
            if (existSpecialization == null)
                return null;

            existSpecialization.Name = dto.Name;
            _specializationRepo.Update(existSpecialization);

            var specializationDto = _mapper.Map<SpecializationReviewDto>(existSpecialization);
            var Saved = await _specializationRepo.SaveChangesAsync();
            if (!Saved)
                return null;

            return specializationDto;
        }

        public async Task<Result> DeleteById(int id)
        {
            var existSpecialization = await _specializationRepo.GetByIdAsync(id);
            if(existSpecialization == null)
                return Result.Failure("Specialization not found.");

            if (existSpecialization.Doctors.Any())
            {
                return Result.Failure("Cam't remove specialization because have Doctors.");
            }

            _specializationRepo.Delete(existSpecialization);

            var saved = await _specializationRepo.SaveChangesAsync();
            if (!saved)
                return Result.Failure("Failer to delete specialization.");

            return Result.Success("Specialization deleted successfuly.");
        }
        #endregion
    }
}
