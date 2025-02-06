using System;

namespace QuitSmoking.Domain.Entities
{
    public class SmokingScore
    {
        public int SmokedToday { get; set; }
        public int SmokedBefore { get; set; }
        public int SmokedYesterday { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public DateTime LastSmokedDate { get; set; } = DateTime.Now;
        public int DaysWithoutSmoking => SmokedToday > 0 ? 0 :(int)(Date - LastSmokedDate).TotalDays;
        public bool Success => (SmokedToday < SmokedBefore) || DaysWithoutSmoking > 0;
        public bool HasData => SmokedBefore > 0; // Lógica para HasData
    }
}

