using System;

namespace PharmacyField.Core.Entities
{
    public class AttendanceRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public decimal? CheckInLatitude { get; set; }
        public decimal? CheckInLongitude { get; set; }
        public decimal? CheckOutLatitude { get; set; }
        public decimal? CheckOutLongitude { get; set; }
        public string? CheckInAddress { get; set; }
        public string? CheckOutAddress { get; set; }
        public string Status { get; set; } = "CheckedIn";
        public decimal? WorkingHours { get; set; }
        public string? Notes { get; set; }
        public string? SelfieBase64 { get; set; } // Add this
        public DateTime CreatedAt { get; set; }

        public virtual User User { get; set; } = null!;
    }
}