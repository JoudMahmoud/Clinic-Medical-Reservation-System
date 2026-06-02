using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Domain.Entities
{
    public class Branch:Base
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }
        public TimeOnly OpeningTime { get; set; }
        public TimeOnly ClosingTime { get; set; }

        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
        public virtual ICollection<Schedule> Schedules { get; set; } = new HashSet<Schedule>(); 
        public virtual ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();

    }
}
