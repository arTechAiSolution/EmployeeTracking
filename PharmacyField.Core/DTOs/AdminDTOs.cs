using System;
using System.Collections.Generic;

namespace PharmacyField.Core.DTOs
{
    // Admin Dashboard Statistics
    public class AdminDashboardStatsDto
    {
        public int TotalEmployees { get; set; }
        public int ActiveEmployees { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalVisitsToday { get; set; }
        public AdminAttendanceSummaryDto AttendanceSummary { get; set; } = new();
    }

    // Admin Attendance Summary
    public class AdminAttendanceSummaryDto
    {
        public int TotalEmployees { get; set; }
        public int CheckedInToday { get; set; }
        public int CheckedOutToday { get; set; }
        public int AbsentToday { get; set; }
        public decimal AverageWorkingHours { get; set; }
    }

    // Admin Employee Response
    public class AdminEmployeeResponseDto
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public string? ProfileImageUrl { get; set; }
    }

    // Admin Employee Update Request
    public class AdminEmployeeUpdateRequestDto
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }
        public bool? IsActive { get; set; }
        public string? ProfileImageUrl { get; set; }
    }

    // Admin Employee Create Request
    public class AdminEmployeeCreateRequestDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } = "Employee";
        public string Password { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
    }

    // Admin Employee Location Response
    public class AdminEmployeeLocationDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? Address { get; set; }
        public DateTime Timestamp { get; set; }
    }

    // Admin Attendance Record Response
    public class AdminAttendanceRecordDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal? WorkingHours { get; set; }
        public string? CheckInAddress { get; set; }
        public string? CheckOutAddress { get; set; }
        public bool HasSelfie { get; set; }
    }

    // Admin Employee Summary
    public class AdminEmployeeSummaryDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string EmployeeCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public decimal? WorkingHours { get; set; }
    }
}