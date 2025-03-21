﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuitSmoking.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<SmokingHistory> SmokingHistories { get; set; }
        public DateTime BirthDate { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        [ForeignKey("Cigarretes")]
        public int? CigarreteId { get; set; }
        public virtual UserCigarrete? Cigarretes { get; set; }
        public int CigarretesPerDay { get; set; } = 0;
        public DateTime RegisteredAt { get; set; }
        public int WeeksUntilNow => (DateTime.Now - RegisteredAt).Days / 7;
    }
}
