using System.ComponentModel.DataAnnotations.Schema;

namespace QuitSmoking.Domain.Entities;

public class SmokingHistory
{
    public int Id { get; set; }

    [ForeignKey("ApplicationUser")]
    public string UserId { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }

    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}
