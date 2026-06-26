using AutoMapper;
using ClinicMedicalReservationSystem.Application.DTOs;
using ClinicMedicalReservationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Application.Automapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //createMap<source, distination>
            CreateMap<RegisterUserDto, Patient>();
            CreateMap<SpecializationDto, Specialization>();
            CreateMap<Specialization, SpecializationReviewDto>();
        }

    }
}
