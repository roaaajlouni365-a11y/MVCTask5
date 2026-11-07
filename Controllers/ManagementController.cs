using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using UniversityCafeteriaManagement.Data;
using UniversityCafeteriaManagement.Models.DatabaseFirst;

namespace UniversityCafeteriaManagement.Controllers;

[Authorize]
public class ManagementController(CafeteriaDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewBag.Categories = await db.Categories.OrderBy(x => x.Name).ToListAsync();
        ViewBag.Suppliers = await db.Suppliers.OrderBy(x => x.Name).ToListAsync();
        ViewBag.Products = await db.Products.Include(x => x.Category).Include(x => x.Supplier).OrderBy(x => x.Name).ToListAsync();
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveCategory(int? id, string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name)) return BadRequest("Category name is required.");
        var item = id.HasValue ? await db.Categories.FindAsync(id.Value) : new Category();
        if (item is null) return NotFound(); item.Name = name.Trim(); item.Description = description?.Trim();
        if (!id.HasValue) db.Categories.Add(item); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveSupplier(int? id, string name, string? email, string? phone)
    {
        if (string.IsNullOrWhiteSpace(name)) return BadRequest("Supplier name is required.");
        var item = id.HasValue ? await db.Suppliers.FindAsync(id.Value) : new Supplier();
        if (item is null) return NotFound(); item.Name = name.Trim(); item.Email = email?.Trim(); item.Phone = phone?.Trim();
        if (!id.HasValue) db.Suppliers.Add(item); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveProduct(int? id, string name, decimal price, int quantity, int categoryId, int supplierId)
    {
        if (string.IsNullOrWhiteSpace(name) || price < 0 || quantity < 0) return BadRequest("Enter a valid product name, price, and quantity.");
        if (!await db.Categories.AnyAsync(x => x.Id == categoryId) || !await db.Suppliers.AnyAsync(x => x.Id == supplierId)) return BadRequest("Select a valid category and supplier.");
        var item = id.HasValue ? await db.Products.FindAsync(id.Value) : new Product();
        if (item is null) return NotFound(); item.Name = name.Trim(); item.Price = price; item.Quantity = quantity; item.CategoryId = categoryId; item.SupplierId = supplierId;
        if (!id.HasValue) db.Products.Add(item); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCategory(int id) { var x = await db.Categories.FindAsync(id); if (x is null) return NotFound(); db.Categories.Remove(x); return await DeleteAndReturn(); }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteSupplier(int id) { var x = await db.Suppliers.FindAsync(id); if (x is null) return NotFound(); db.Suppliers.Remove(x); return await DeleteAndReturn(); }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProduct(int id) { var x = await db.Products.FindAsync(id); if (x is null) return NotFound(); db.Products.Remove(x); return await DeleteAndReturn(); }

    private async Task<IActionResult> DeleteAndReturn()
    {
        try { await db.SaveChangesAsync(); }
        catch (DbUpdateException) { TempData["Error"] = "This record is referenced by another record and cannot be deleted."; }
        return RedirectToAction(nameof(Index));
    }
}
