using AutoMapper;
using ClinicMedicalReservationSystem.Application.Common;
using ClinicMedicalReservationSystem.Application.DTOs;
using ClinicMedicalReservationSystem.Application.Exceptions;
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
        public async Task<IEnumerable<SpecializationReviewDto>> GetAllAsync(string? search)
        {
            var specializations = await _specializationRepo.GetAllAsync(search);
            
            if(!specializations.Any()) 
                return Enumerable.Empty<SpecializationReviewDto>();

            var SpecializationDtos = _mapper.Map<IEnumerable<SpecializationReviewDto>>(specializations);
            return SpecializationDtos;

        }
        public async Task<Result> AddAsync(SpecializationDto dto)
        {
            var existSpecialization = await _specializationRepo.GetByNameAsync(dto.Name);
            if (existSpecialization != null)
                return Result.Failure("Specialization is exist already.");

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
                throw new NotFoundException("Specialization not found.");

            var specializationDto = _mapper.Map<SpecializationReviewDto>(exsitSpecialization);
            return specializationDto;
        }

        public async Task<SpecializationReviewDto> UpdateAsync(int id, SpecializationDto dto)
        {
            var existSpecialization = await _specializationRepo.GetByIdAsync(id);
            if (existSpecialization == null)
                throw new NotFoundException("Specialization not found.");
            var specializationWithSameName = await _specializationRepo.GetByNameAsync(dto.Name);
            if (specializationWithSameName != null)
                throw new NotFoundException("There is a specialization by the same name.");
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
            var existSpecialization = await _specializationRepo.GetByIdWithDoctorsAsync(id);
            if(existSpecialization == null)
                throw new NotFoundException("Specialization not found.");

            if (existSpecialization.Doctors.Any())
            {
                return Result.Failure("Cam't delete pecialization because it has Doctors.");
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
