using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuitSmoking.Domain.Entities;

public class SmokingHistory
{
    public int Id { get; set; }

    [ForeignKey("ApplicationUser")]
    public string UserId { get; set; }
    [JsonIgnore]
    public virtual ApplicationUser ApplicationUser { get; set; }

    [ForeignKey("UserCigarrete")]
    public int CigarreteId { get; set; }
    public virtual UserCigarrete Cigarrete { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}
