﻿using QuitSmoking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuitSmoking.Application.DTOs
{
    public class SmokingHistoryDto
    {
        public int CigarretesId { get; set; }
        public int Quantity { get; set; }
    }
}
