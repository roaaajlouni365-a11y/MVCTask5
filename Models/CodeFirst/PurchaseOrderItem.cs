using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace UniversityCafeteriaManagement.Models.CodeFirst;
public class PurchaseOrderItem { public int Id { get; set; } public int PurchaseOrderId { get; set; } public int ProductId { get; set; } [Range(1, int.MaxValue)] public int Quantity { get; set; } [Column(TypeName = "decimal(10,2)")] public decimal UnitPrice { get; set; } public PurchaseOrder PurchaseOrder { get; set; } = null!; public Product Product { get; set; } = null!; }
