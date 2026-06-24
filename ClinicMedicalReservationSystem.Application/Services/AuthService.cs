using AutoMapper;
using ClinicMedicalReservationSystem.Application.Common;
using ClinicMedicalReservationSystem.Application.DTOs;
using ClinicMedicalReservationSystem.Application.Interfaces;
using ClinicMedicalReservationSystem.Domain.Entities;
using ClinicMedicalReservationSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
//using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace ClinicMedicalReservationSystem.Application.Services
{
    public class AuthService:IAuthService
    {
        #region Fields 
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _config;
        private readonly IRoleService _roleService;
        private readonly IRefreshTokenRepo _refreshTokenRepo;
        
        #endregion

        #region Constructor
        public AuthService(UserManager<User> userManager, IMapper mapper, ILogger<AuthService> logger, IConfiguration config,IRoleService roleService, IRefreshTokenRepo refreshTokenRepo)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _config = config;
            _roleService = roleService;
            _refreshTokenRepo = refreshTokenRepo;
        }
        #endregion

        #region CRUD Operations
        public async Task<Result> RegisterAsync(RegisterUserDto registerUserDto)
        {
          
                var existedUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == registerUserDto.Email);
                if (existedUser != null)
                    return Result.Failure("Email is already in use.");
                existedUser = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == registerUserDto.PhoneNumber);
                if (existedUser != null)
                    return Result.Failure("Phone number is already in use.");

                var patient = _mapper.Map<Patient>(registerUserDto);
                IdentityResult result = await _userManager.CreateAsync(patient, registerUserDto.Password); 
                if (result.Succeeded)
                {
                    await _roleService.AddUserToRoleAsync(patient, "Patient");
                    return Result.Success("User added successfuly.");
                }
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result.Failure(errors);
            }
           
        public async Task<LoginResponseDto?> LoginAsync(LoginUserDto dto)
        {
          
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == dto.EmailOrPhoneNumber);
                if (user == null)
                    user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == dto.EmailOrPhoneNumber);

                if (user == null)
                    return null;

                var validPassword = await _userManager.CheckPasswordAsync(user, dto.Password);

                if (!validPassword)
                    return null;

                
                var token = GenerateJwtToken(user);
                var refreshTokenEntity = new RefreshToken
                {
                    Token = token.RefreshToken,
                    UserId = user.Id,
                    CreatedOn = DateTime.UtcNow,
                    ExpiresOn = DateTime.UtcNow.AddDays(15),
                    RevokedOn= null
                };

                await _refreshTokenRepo.AddAsync(refreshTokenEntity);
                var saved = await _refreshTokenRepo.SaveChangesAsync();
            if (!saved)
                throw new Exception("Failed to save refresh token.");

                return token;
            }

        public async Task<LoginResponseDto?> RefreshTokenAsync(string refreshToken)
        {

            var tokenExist = await _refreshTokenRepo.GetByTokenAsync(refreshToken);

            if (tokenExist == null)
                return null;

            if (tokenExist.RevokedOn != null)
                return null;

            if (tokenExist.ExpiresOn <= DateTime.UtcNow)
                return null;

            var user = await _userManager.FindByIdAsync(tokenExist.UserId);

            if (user == null)
                return null;

            var newTokens = GenerateJwtToken(user);

            tokenExist.RevokedOn = DateTime.UtcNow;

            var refreshTokenEntity = new RefreshToken
            {
                Token = newTokens.RefreshToken,
                UserId = user.Id,
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(15),
                RevokedOn = null
            };

            await _refreshTokenRepo.AddAsync(refreshTokenEntity);

            var isSaved = await _refreshTokenRepo.SaveChangesAsync();
            if (!isSaved)
                throw new Exception("Failed to save refresh token.");

            return newTokens;

        }
   
        public async Task LogoutAsync(string userId, string refreshToken)
        {
            var tokenExist = await _refreshTokenRepo.GetByTokenandUserIdAsync(userId, refreshToken);
            if (tokenExist == null)
                return;

            tokenExist.RevokedOn = DateTime.UtcNow;

            var isSaved = await _refreshTokenRepo.SaveChangesAsync();
            if (!isSaved)
                return;
        }
        
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private LoginResponseDto GenerateJwtToken(User user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("userId", user.Id),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));

            var signingCredentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            var expirationAccessToken = DateTime.UtcNow.AddMinutes(30);
            var expirationRefreshToken = DateTime.UtcNow.AddDays(15);

            
            var accessToken = new JwtSecurityToken(
            issuer: _config["JWT:ValidIssuer"],
            audience: _config["JWT:ValidAudience"],
            claims: claims,
            expires: expirationAccessToken,
            signingCredentials: signingCredentials);


            var refreshToken = GenerateRefreshToken();
     
            return new LoginResponseDto
            {
               AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
               RefreshToken = refreshToken,
               ExpirationAccessToken = expirationAccessToken,
               ExpirationRefreshToken = expirationRefreshToken
            };

        }
        #endregion
    }



}
