using System.ComponentModel.DataAnnotations.Schema;

namespace QuitSmoking.Domain.Entities;

public class UserCigarrete
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public double PricePerBox { get; set; }
    [ForeignKey("ApplicationUserId")]
    public string ApplicationUserId { get; set; } // Alterado para GUID
    public virtual ApplicationUser ApplicationUser { get; set; }
}
