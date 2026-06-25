using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Application.DTOs
{
    public class SpecializationDto
    {
        [Required]
        public string Name { get; set; }

    }
}
