using System.ComponentModel.DataAnnotations;
namespace UniversityCafeteriaManagement.Models.PurchaseOrders;
public class PurchaseOrderCreateViewModel
{
    [Required] public int SupplierId { get; set; }
    public List<PurchaseOrderLineInput> Lines { get; set; } = new() { new() };
}
public class PurchaseOrderLineInput
{
    public int ProductId { get; set; }
    [Range(1, int.MaxValue)] public int Quantity { get; set; }
}
