using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace UniversityCafeteriaManagement.Models.DatabaseFirst;
public class Product { public int Id { get; set; } [Required, StringLength(150)] public string Name { get; set; } = ""; [Range(0, 99999999.99), Column(TypeName = "decimal(10,2)")] public decimal Price { get; set; } [Range(0, int.MaxValue)] public int Quantity { get; set; } public int CategoryId { get; set; } public int SupplierId { get; set; } public Category Category { get; set; } = null!; public Supplier Supplier { get; set; } = null!; public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>(); }
