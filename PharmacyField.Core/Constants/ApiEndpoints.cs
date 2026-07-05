namespace PharmacyField.Core.Constants
{
    public static class ApiEndpoints
    {
        // Admin Endpoints
        public const string AdminDashboardStats = "api/admin/dashboard/stats";
        public const string AdminEmployees = "api/admin/employees";
        public const string AdminEmployeeById = "api/admin/employees/{id}";
        public const string AdminEmployeeUpdate = "api/admin/employees/{id}";
        public const string AdminEmployeeDelete = "api/admin/employees/{id}";
        public const string AdminEmployeePermanentDelete = "api/admin/employees/{id}/permanent";
        public const string AdminLocations = "api/admin/locations";
        public const string AdminLocationHistory = "api/admin/locations/employee/{userId}";
        public const string AdminAttendanceSummary = "api/admin/attendance/summary";
        public const string AdminAttendanceRecords = "api/admin/attendance/records";
        public const string AdminEmployeeAttendance = "api/admin/attendance/employee/{userId}";
        public const string AdminAttendanceSelfie = "api/admin/attendance/selfie/{attendanceId}";
        public const string AdminTodayAttendanceSummary = "api/admin/attendance/today-summary";
        public const string AdminRecentActivity = "api/admin/activity/recent";
    }
}