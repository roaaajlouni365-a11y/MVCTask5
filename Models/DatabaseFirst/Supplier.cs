using System.ComponentModel.DataAnnotations;
namespace UniversityCafeteriaManagement.Models.DatabaseFirst;
public class Supplier { public int Id { get; set; } [Required, StringLength(150)] public string Name { get; set; } = ""; [StringLength(254), EmailAddress] public string? Email { get; set; } [StringLength(30)] public string? Phone { get; set; } public ICollection<Product> Products { get; set; } = new List<Product>(); public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>(); }
