using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Application.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [RegularExpression(@"^01\d{9}$", ErrorMessage = "Invalid Phone number.")]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        [Required]
        //[Length(8, 100)]
        [RegularExpression(
            @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&*]).{8,10}$",
            ErrorMessage = "Password must be at range 8 and 10 characters and contain letters, numbers, and special characters."
        )]
        public string Password { get; set; }
        public string? ImageUrl { get; set; }
        public char Gender { get; set; }
        public DateOnly BrithDate { get; set; }
        public int EmergencyContact { get; set; }
    }
}