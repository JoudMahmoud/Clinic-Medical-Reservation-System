using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Domain.Entities
{
    public class Doctor : User
    {
        public int YearsOfExperience {  get; set; }
        public int Fees {  get; set; }
        public string Bio {  get; set; } = string.Empty;

        [Required]
        [ForeignKey("Specialization")]
        public int SpecializationId { get; set; }
        public virtual Specialization Specialization { get; set; }

        public virtual ICollection<Branch> Branches { get; set; } = new HashSet<Branch>();
        public virtual ICollection<Schedule> Schedules { get; set; } = new HashSet<Schedule>();
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public virtual ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();

    }
}
