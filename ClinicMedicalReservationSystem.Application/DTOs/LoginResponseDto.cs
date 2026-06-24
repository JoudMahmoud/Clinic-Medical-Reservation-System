using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Application.DTOs
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public DateTime ExpirationAccessToken {  get; set; }
        public DateTime ExpirationRefreshToken { get; set; }

    }
}
