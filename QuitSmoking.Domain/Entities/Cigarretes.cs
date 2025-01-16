using System.ComponentModel.DataAnnotations.Schema;

namespace QuitSmoking.Domain.Entities;

public class Cigarretes
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public double PricePerBox { get; set; }

    [InverseProperty("Cigarretes")]
    public virtual List<SmokingHistory> SmokingHistories { get; set; }
}
