using ClinicMedicalReservationSystem.Application.DTOs;
using ClinicMedicalReservationSystem.Application.Interfaces;
using ClinicMedicalReservationSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace ClinicMedicalReservationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Fields
        private readonly IAuthService _authService;
        #endregion

        #region Constructor
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        #endregion

        #region CRUD Operations
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDto dto)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);  

            var result = await _authService.RegisterAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.Message });

            return Ok(new {message = result.Message});
        }

        [HttpPost("login")]
        [EnableRateLimiting("login")]  
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserDto userDto)
        {
            var result = await _authService.LoginAsync(userDto);

            if(!result)
                return Unauthorized();

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody]string refreshToken)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken);
            if (result == null)
                return Unauthorized();
            return Ok(result);
        }

        [Authorize]
        [HttpPost("log-out")]
        public async Task<IActionResult> LogOutAsync([FromBody] string dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();
            await _authService.LogoutAsync(userId, dto);
            return Ok();

        }
        #endregion
    }
}
