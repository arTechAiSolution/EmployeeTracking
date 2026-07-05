using System;

namespace PharmacyField.Core.DTOs
{
    public class DoctorRequestDto
    {
        public string DoctorName { get; set; } = string.Empty;
        public string? Specialization { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? ClinicName { get; set; }
        public string? ClinicAddress { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? VisitFrequency { get; set; }
    }
    
    public class DoctorResponseDto
    {
        public int Id { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string? Specialization { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? ClinicName { get; set; }
        public string? ClinicAddress { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? VisitFrequency { get; set; }
        public int TotalVisits { get; set; }
    }
    
    public class DoctorVisitRequestDto
    {
        public int DoctorId { get; set; }
        public string VisitType { get; set; } = "Regular";
        public string? Purpose { get; set; }
        public string? Notes { get; set; }
        public bool SamplesGiven { get; set; }
        public string? SamplesDescription { get; set; }
        public bool OrderTaken { get; set; }
        public decimal? OrderAmount { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}