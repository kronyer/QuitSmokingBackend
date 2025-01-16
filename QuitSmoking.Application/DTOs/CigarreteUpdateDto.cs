using QuitSmoking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuitSmoking.Application.DTOs
{
    public class CigarreteUpdateDto
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public double PricePerBox { get; set; }
    }
}
