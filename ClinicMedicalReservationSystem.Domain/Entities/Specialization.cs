using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Domain.Entities
{
    public class Specialization:Base
    {
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Doctor> Doctors { get; set; } = new HashSet<Doctor>();
    }
}
