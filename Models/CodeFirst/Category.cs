using System.ComponentModel.DataAnnotations;
namespace UniversityCafeteriaManagement.Models.CodeFirst;
public class Category { public int Id { get; set; } [Required, StringLength(100)] public string Name { get; set; } = ""; [StringLength(500)] public string? Description { get; set; } public ICollection<Product> Products { get; set; } = new List<Product>(); }
