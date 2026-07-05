using System;

namespace PharmacyField.Core.Entities
{
    public class DoctorVisit
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public DateTime VisitDate { get; set; }
        public string VisitType { get; set; } = "Regular";
        public string? Purpose { get; set; }
        public string? Notes { get; set; }
        public bool SamplesGiven { get; set; }
        public string? SamplesDescription { get; set; }
        public bool OrderTaken { get; set; }
        public decimal? OrderAmount { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string Status { get; set; } = "Scheduled";
        public DateTime CreatedAt { get; set; }
        
        public virtual Doctor Doctor { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}