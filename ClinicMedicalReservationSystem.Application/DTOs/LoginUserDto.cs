using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Application.DTOs
{
    public class LoginUserDto
    {
        public string EmailOrPhoneNumber { get; set; }
        public string Password { get; set; }
      
    }
}
