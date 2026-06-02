using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Domain.Entities
{
    public class ScheduleChangeRequest:Base
    {
        [Required]
        public string Day {  get; set; }
        public TimeOnly NewStartingTime { get; set; }
        public TimeOnly NewEndingTime { get; set; }


        [Required]
        [ForeignKey("Schedule")]
        public int ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }
    }
}
