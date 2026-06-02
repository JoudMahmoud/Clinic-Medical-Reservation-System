using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicMedicalReservationSystem.Domain.Entities
{
    public class Patient:User
    {
        public char Gender { get; set; }
        public DateOnly BrithDate { get; set; }
        public int EmergencyContact {  get; set; }

        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public virtual ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();

    }
}
