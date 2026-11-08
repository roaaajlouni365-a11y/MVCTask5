using CafeteriaManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace CafeteriaManagement.CodeFirst.Models;

public class PurchaseOrder
{
    public int Id { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Required]
    [StringLength(30)]
    public string Status { get; set; } = "Pending";

    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;

    public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        = new List<PurchaseOrderItem>();
}