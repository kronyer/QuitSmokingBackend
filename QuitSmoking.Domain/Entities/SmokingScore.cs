using System;

namespace QuitSmoking.Domain.Entities
{
    public class SmokingScore
    {
        public int SmokedToday { get; set; }
        public int SmokedBefore { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public DateTime LastSmokedDate { get; set; } = DateTime.Now;
        public bool Success => SmokedToday < SmokedBefore;
        public bool HasData => SmokedBefore > 0; // Lógica para HasData
    }
}

