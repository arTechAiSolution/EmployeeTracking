using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyField.Core.DTOs;
using PharmacyField.Core.Entities;
using PharmacyField.Infrastructure.Data;
using System.Security.Claims;

namespace PharmacyField.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }

        // Get local time (IST - Indian Standard Time)
        private DateTime GetCurrentLocalTime()
        {
            return DateTime.Now;
        }

        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn([FromBody] CheckInRequestDto request)
        {
            try
            {
                var userId = GetUserId();
                var today = GetCurrentLocalTime().Date;

                // Check if user exists
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    return NotFound(new { message = "User not found" });

                // Check if already checked in today
                var existing = await _context.AttendanceRecords
                    .FirstOrDefaultAsync(a => a.UserId == userId && a.CheckInTime.Date == today);

                if (existing != null)
                    return BadRequest(new { message = "Already checked in today" });

                // Use client time if provided, otherwise use server local time
                var checkInTime = request.CheckInTime ?? GetCurrentLocalTime();

                var attendance = new AttendanceRecord
                {
                    UserId = userId,
                    CheckInTime = checkInTime,
                    CheckInLatitude = request.Latitude,
                    CheckInLongitude = request.Longitude,
                    CheckInAddress = request.Address,
                    Notes = request.Notes,
                    SelfieBase64 = request.SelfieBase64,
                    Status = "CheckedIn",
                    CreatedAt = GetCurrentLocalTime()
                };

                await _context.AttendanceRecords.AddAsync(attendance);
                await _context.SaveChangesAsync();

                return Ok(new AttendanceResponseDto
                {
                    Id = attendance.Id,
                    CheckInTime = attendance.CheckInTime,
                    Status = attendance.Status,
                    CheckInAddress = attendance.CheckInAddress,
                    SelfieUrl = attendance.SelfieBase64 != null ? "Selfie captured" : null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error during check-in: {ex.Message}" });
            }
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOut([FromBody] CheckOutRequestDto request)
        {
            try
            {
                var userId = GetUserId();
                var today = GetCurrentLocalTime().Date;

                // Check if user exists
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    return NotFound(new { message = "User not found" });

                // Find active check-in for today
                var attendance = await _context.AttendanceRecords
                    .FirstOrDefaultAsync(a => a.UserId == userId && a.CheckInTime.Date == today && a.Status == "CheckedIn");

                if (attendance == null)
                    return BadRequest(new { message = "No active check-in found. Please check in first." });

                // Use client time if provided, otherwise use server local time
                var checkOutTime = request.CheckOutTime ?? GetCurrentLocalTime();

                attendance.CheckOutTime = checkOutTime;
                attendance.CheckOutLatitude = request.Latitude;
                attendance.CheckOutLongitude = request.Longitude;
                attendance.CheckOutAddress = request.Address;
                attendance.Notes = string.IsNullOrEmpty(attendance.Notes)
                    ? request.Notes
                    : $"{attendance.Notes}\n{request.Notes}";
                attendance.SelfieBase64 = request.SelfieBase64 ?? attendance.SelfieBase64;
                attendance.Status = "CheckedOut";

                // Calculate working hours
                var hours = (attendance.CheckOutTime.Value - attendance.CheckInTime).TotalHours;
                attendance.WorkingHours = (decimal)Math.Round(hours, 2);

                await _context.SaveChangesAsync();

                return Ok(new AttendanceResponseDto
                {
                    Id = attendance.Id,
                    CheckInTime = attendance.CheckInTime,
                    CheckOutTime = attendance.CheckOutTime,
                    Status = attendance.Status,
                    WorkingHours = attendance.WorkingHours,
                    CheckInAddress = attendance.CheckInAddress,
                    CheckOutAddress = attendance.CheckOutAddress,
                    SelfieUrl = attendance.SelfieBase64 != null ? "Selfie captured" : null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error during check-out: {ex.Message}" });
            }
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetTodayAttendance()
        {
            try
            {
                var userId = GetUserId();
                var today = GetCurrentLocalTime().Date;

                var attendance = await _context.AttendanceRecords
                    .FirstOrDefaultAsync(a => a.UserId == userId && a.CheckInTime.Date == today);

                if (attendance == null)
                    return Ok(null);

                return Ok(new AttendanceResponseDto
                {
                    Id = attendance.Id,
                    CheckInTime = attendance.CheckInTime,
                    CheckOutTime = attendance.CheckOutTime,
                    Status = attendance.Status,
                    WorkingHours = attendance.WorkingHours,
                    CheckInAddress = attendance.CheckInAddress,
                    CheckOutAddress = attendance.CheckOutAddress,
                    SelfieUrl = attendance.SelfieBase64 != null ? "Selfie captured" : null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error fetching attendance: {ex.Message}" });
            }
        }

        //[HttpGet("history")]
        //public async Task<IActionResult> GetAttendanceHistory(
        //    [FromQuery] int page = 1,
        //    [FromQuery] int pageSize = 10,
        //    [FromQuery] int? month = null,
        //    [FromQuery] int? year = null)
        //{
        //    try
        //    {
        //        var userId = GetUserId();

        //        var query = _context.AttendanceRecords
        //            .Where(a => a.UserId == userId);

        //        if (month.HasValue && year.HasValue)
        //        {
        //            query = query.Where(a => a.CheckInTime.Month == month.Value && a.CheckInTime.Year == year.Value);
        //        }

        //        var totalCount = await query.CountAsync();

        //        var history = await query
        //            .OrderByDescending(a => a.CheckInTime)
        //            .Skip((page - 1) * pageSize)
        //            .Take(pageSize)
        //            .Select(a => new AttendanceHistoryResponseDto
        //            {
        //                Id = a.Id,
        //                Date = a.CheckInTime.Date,
        //                CheckInTime = a.CheckInTime,
        //                CheckOutTime = a.CheckOutTime,
        //                WorkingHours = a.WorkingHours,
        //                Status = a.Status,
        //                HasSelfie = a.SelfieBase64 != null
        //            })
        //            .ToListAsync();

        //        return Ok(new
        //        {
        //            data = history,
        //            total = totalCount,
        //            page = page,
        //            pageSize = pageSize,
        //            totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = $"Error fetching history: {ex.Message}" });
        //    }
        //}

        [HttpGet("selfie/{attendanceId}")]
        public async Task<IActionResult> GetSelfie(int attendanceId)
        {
            try
            {
                var userId = GetUserId();

                var attendance = await _context.AttendanceRecords
                    .FirstOrDefaultAsync(a => a.Id == attendanceId && a.UserId == userId);

                if (attendance == null)
                    return NotFound(new { message = "Attendance record not found" });

                if (string.IsNullOrEmpty(attendance.SelfieBase64))
                    return NotFound(new { message = "No selfie found for this attendance" });

                return Ok(new { selfie = attendance.SelfieBase64 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error fetching selfie: {ex.Message}" });
            }
        }

        [HttpGet("ischeckedin")]
        public async Task<IActionResult> IsCheckedInToday()
        {
            try
            {
                var userId = GetUserId();
                var today = GetCurrentLocalTime().Date;

                var isCheckedIn = await _context.AttendanceRecords
                    .AnyAsync(a => a.UserId == userId && a.CheckInTime.Date == today && a.Status == "CheckedIn");

                return Ok(new { isCheckedIn = isCheckedIn });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error checking status: {ex.Message}" });
            }
        }
    }
}