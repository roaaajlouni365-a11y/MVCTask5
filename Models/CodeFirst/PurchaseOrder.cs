using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace UniversityCafeteriaManagement.Models.CodeFirst;
public class PurchaseOrder { public int Id { get; set; } public DateTime OrderDate { get; set; } [Required, StringLength(20)] public string Status { get; set; } = "Pending"; public int SupplierId { get; set; } public Supplier Supplier { get; set; } = null!; public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>(); [NotMapped] public decimal Total => Items.Sum(item => item.Quantity * item.UnitPrice); }
