using ClinicMedicalReservationSystem.Application.Common;
using ClinicMedicalReservationSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result> RegisterAsync(RegisterUserDto registerUserDto);
        Task<LoginResponseDto?> LoginAsync(LoginUserDto dto);
        Task<LoginResponseDto> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string userId, string refreshToken);
    }
}
