using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityCafeteriaManagement.Data;
using UniversityCafeteriaManagement.Models.DatabaseFirst;
using UniversityCafeteriaManagement.Models.PurchaseOrders;

namespace UniversityCafeteriaManagement.Controllers;

[Authorize]
public class PurchaseOrdersController(CafeteriaDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.PurchaseOrders.Include(x => x.Supplier).Include(x => x.Items).OrderByDescending(x => x.OrderDate).ToListAsync());

    public async Task<IActionResult> Create()
    {
        await LoadSuppliers();
        ViewBag.Products = await db.Products.OrderBy(x => x.Name).ToListAsync();
        return View(new PurchaseOrderCreateViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PurchaseOrderCreateViewModel input)
    {
        var lines = input.Lines.Where(x => x.ProductId > 0 && x.Quantity > 0).ToList();
        if (lines.Count == 0) ModelState.AddModelError(nameof(input.Lines), "Add at least one product.");
        if (lines.Select(x => x.ProductId).Distinct().Count() != lines.Count) ModelState.AddModelError(nameof(input.Lines), "Each product may appear once in an order.");
        var products = await db.Products.Where(x => lines.Select(y => y.ProductId).Contains(x.Id)).ToListAsync();
        if (products.Count != lines.Select(x => x.ProductId).Distinct().Count() || products.Any(x => x.SupplierId != input.SupplierId)) ModelState.AddModelError(nameof(input.Lines), "Select products supplied by the selected supplier.");
        if (!ModelState.IsValid)
        {
            await LoadSuppliers(); ViewBag.Products = await db.Products.OrderBy(x => x.Name).ToListAsync(); return View(input);
        }

        var order = new PurchaseOrder { SupplierId = input.SupplierId, OrderDate = DateTime.UtcNow, Status = "Pending" };
        foreach (var line in lines)
        {
            var product = products.Single(x => x.Id == line.ProductId);
            order.Items.Add(new PurchaseOrderItem { ProductId = product.Id, Quantity = line.Quantity, UnitPrice = product.Price });
        }
        db.PurchaseOrders.Add(order);
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = order.Id });
    }

    public async Task<IActionResult> Details(int id)
    {
        var order = await db.PurchaseOrders.Include(x => x.Supplier).Include(x => x.Items).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.Id == id);
        return order is null ? NotFound() : View(order);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Receive(int id)
    {
        await using var transaction = await db.Database.BeginTransactionAsync();
        var order = await db.PurchaseOrders.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id);
        if (order is null) return NotFound();
        if (order.Status != "Pending") { TempData["Error"] = "Only pending orders can be received."; return RedirectToAction(nameof(Details), new { id }); }
        var quantities = order.Items.GroupBy(x => x.ProductId).ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));
        var products = await db.Products.Where(x => quantities.Keys.Contains(x.Id)).ToListAsync();
        foreach (var product in products) product.Quantity = checked(product.Quantity + quantities[product.Id]);
        order.Status = "Received";
        await db.SaveChangesAsync();
        await transaction.CommitAsync();
        return RedirectToAction(nameof(Details), new { id });
    }

    private async Task LoadSuppliers() => ViewBag.Suppliers = new SelectList(await db.Suppliers.OrderBy(x => x.Name).ToListAsync(), "Id", "Name");
}
