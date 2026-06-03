using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Domain.Entities
{
    public class Schedule:Base
    {
        [Required]
        public string Day {  get; set; }
        public TimeOnly StartingTime { get; set; }
        public TimeOnly EndingTime { get; set; }


        [Required]
        [ForeignKey("Doctor")]
        public string DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }


        [Required]
        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public virtual Branch Branch { get; set; }

        public virtual ICollection<ScheduleChangeRequest> ScheduleChanges { get; set; } = new HashSet<ScheduleChangeRequest>();
    }
}
