using System;
using System.Collections.Generic;

namespace PharmacyField.Core.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string? Specialization { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? ClinicName { get; set; }
        public string? ClinicAddress { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? VisitFrequency { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public virtual User User { get; set; } = null!;
        public virtual ICollection<DoctorVisit> DoctorVisits { get; set; } = new List<DoctorVisit>();
    }
}