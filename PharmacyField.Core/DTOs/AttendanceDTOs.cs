using System;

namespace PharmacyField.Core.DTOs
{
    public class CheckInRequestDto
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? Address { get; set; }
        public string? Notes { get; set; }
        public DateTime? CheckInTime { get; set; }
        public string? SelfieBase64 { get; set; } // Add this
    }

    public class CheckOutRequestDto
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? Address { get; set; }
        public string? Notes { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string? SelfieBase64 { get; set; } // Add this
    }

    public class AttendanceResponseDto
    {
        public int Id { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal? WorkingHours { get; set; }
        public string? CheckInAddress { get; set; }
        public string? CheckOutAddress { get; set; }
        public string? SelfieUrl { get; set; } // Optional: URL to stored selfie
    }
}