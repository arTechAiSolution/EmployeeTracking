using System;

namespace PharmacyField.Core.Entities
{
    public class LocationTrack
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal? Altitude { get; set; }
        public decimal? Speed { get; set; }
        public decimal? Heading { get; set; }
        public decimal? Accuracy { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsSynced { get; set; }
        public string? SessionId { get; set; }
        
        public virtual User User { get; set; } = null!;
    }
}